using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EC226.Auth;
using EC226.Configuration;
using EC226.Core.Config;
using EC226.Filter;
using EC226.Loging;
using EC226.Mapping;
using EC226.Middlewares;
using EC226.Swagger;
using EC226.Task;
using CorrelationId;
using CorrelationId.DependencyInjection;
using CorrelationId.HttpClient;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;
using SqlSugar.IOC;
using EC226.MemberSite.Api.AutoFac;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using EC226.Core.Attribute;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using Stripe;
using EC226.Pagination;
using EC226.MemberSite.Model.ViewModels.Payment;
using AutoMapper;

using EC226.MemberSite.Services.AutoMapperProfile;
using EC226.Utility.Library.Extensions;

// Early init of NLog to allow startup and exception logging, before host is built
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
   var builder = WebApplication.CreateBuilder(args);

   //新增本地路徑取得支援
   builder.Services.AddSingleton(new AppSettingsHelper(builder.Environment.ContentRootPath));
   builder.Services.AddSingleton(new LogLockHelper(builder.Environment.ContentRootPath));
   
   //Memory緩存
   builder.Services.AddMemoryCacheSetup();
   //Redis緩存
   builder.Services.AddRedisCacheSetup();

   //新增資料庫連接SqlSugar註入支援
   //builder.Services.AddSqlSugarSetup();
   //配置跨域（CORS）
   builder.Services.AddCorsSetup();

   //新增session支援(session依賴於cache進行存儲)
   builder.Services.AddSession();
   // AutoMapper支援
   builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));

   //使用 SignalR
   builder.Services.AddSignalR();

   //Redis訊息隊列
   //builder.Services.AddRedisMessageQueueSetup();

   // 註入工廠 HTTP 客戶端
   builder.Services.AddHttpClient();

   // auto mapper
   var _mapperConfiguration = new MapperConfiguration(cfg =>
   {
      cfg.AddProfile(new MapProfile());
   });

   AutoMapperInstance.InitAutoMapperConfig(_mapperConfiguration);

   //Swagger介面文檔註入
   builder.Services.AddClientSwaggerSetup();

   //註冊Hangfire定時任務
   //builder.Services.AddHangFireSetup();

   //授權支援註入
   builder.Services.AddAuthorizationSetupForClient();
   //上下文註入
   builder.Services.AddHttpContextSetup();

   // 分析效能
   builder.Services.AddMiniProfiler(options =>
   {
      options.RouteBasePath = "/profiler";
      // 看分析結果可以到 "/profiler/results-index"
   });

   //服務配置中加入AutoFac控制器替換規則。
   builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

   //註冊mvc，註冊razor引擎視圖
   builder.Services.AddMvc(options =>
   {
      //實體驗證
      // options.Filters.Add<RequiredErrorForClent>();
      //異常處理
      options.Filters.Add<GlobalExceptionsFilterForClent>();
      //Swagger剔除不需要加入api展示的列表
      options.Conventions.Add(new ApiExplorerIgnores());
      options.ModelBinderProviders.Insert(0, new SearchModelBinder.SearchModelBinderProvider());
   })
       .AddNewtonsoftJson(p =>
       {
          //數據格式首字母小寫 不使用駝峰
          p.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
          //不使用駝峰樣式的key
          //p.SerializerSettings.ContractResolver = new DefaultContractResolver();
          //忽略循環引用
          p.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
          //設置時間格式（必須使用yyyy/MM/dd格式，因為ios系統不支援2018-03-29格式的時間，只識別2018/03/09這種格式。）
          p.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";
       });                

   // 鏈路追蹤
   builder.Services.AddDefaultCorrelationId(options =>
   {
      //options.CorrelationIdGenerator = () => "Foo";
      //options.AddToLoggingScope = true;
      //options.EnforceHeader = true;
      //options.IgnoreRequestHeader = false;
      //options.IncludeInResponse = true;
      //options.RequestHeader = "My-Custom-Correlation-Id";
      //options.ResponseHeader = "X-Correlation-Id";
      options.UpdateTraceIdentifier = true;
   });
   // 同一鏈路中所有節點需宣告同一個Client名稱，加入此句才能將CorrelationId傳遞出去
   builder.Services.AddHttpClient("MyClient").AddCorrelationIdForwarding();

   // 關閉預設模型驗證過濾器
   // https://stackoverflow.com/questions/46374994/correct-way-to-disable-model-validation-in-asp-net-core-2-mvc
   builder.Services.Configure<ApiBehaviorOptions>(options =>
   {
      options.SuppressModelStateInvalidFilter = true;
   });

   #region AutoFac註冊============================================================================

   builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
   builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
   {
      //取得所有控制器類型並使用屬性註入
      var controllerBaseType = typeof(ControllerBase);
      containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
           .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
           .PropertiesAutowired();

      containerBuilder.RegisterModule(new AutofacModuleRegister());

   });

   #endregion

   //builder.Services.AddSqlSugar(new SqlSugar.IOC.IocConfig()
   //{
   //   ConfigId = "master1",
   //   DbType = IocDbType.Sqlite,
   //   IsAutoCloseConnection = true,
   //   ConnectionString = "DataSource=" + GlobalConstVars.GetCurrentDirectory() + @"\database\sqlite.db"
   //});

   builder.Services.AddSqlSugar(new IocConfig()
   {
      ConfigId = "master2",
      //資料庫連線
      ConnectionString = AppSettingsConstVars.DbSqlConnection,
      //判斷資料庫型別
      DbType = AppSettingsConstVars.DbDbType == IocDbType.MySql.ToString() ? IocDbType.MySql : IocDbType.SqlServer,
      //是否開啟自動關閉資料庫連線-//不設成true要手動close
      IsAutoCloseConnection = true,
   });

   builder.Services.ConfigurationSugar(db =>
   {
      //if (!db.ConfigQuery.Any())
      //{
      //   db.ConfigQuery.SetTable<Template>(it => it.Id, it => it.Title);
      //   db.ConfigQuery.SetTable<TemplateType>(it => it.Id, it => it.Name);
      //}
   });
   Task.Run(() =>
   {
#if !DEBUG
                Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(200);
                    var url = "http://localhost:5000?id=" + SqlSugar.SnowFlakeSingle.Instance.NextId();
                    try
                    {
                        Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName =url, Arguments = "chrome.exe" });
                    }
                    catch (Exception)
                    {
                        Process.Start("explorer", url);
                    }
                });
#endif
   });
   #region CMS


   #endregion

   // NLog: Setup NLog for Dependency injection
   builder.Logging.ClearProviders();
   builder.Host.UseNLog();

   var app = builder.Build();
   
   #region 解決Ubuntu Nginx 代理不能取得IP問題
   app.UseForwardedHeaders(new ForwardedHeadersOptions
   {
      ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
   });
   #endregion

   // 記錄請求與返回數據 (註意開啟權限，不然本地無法寫入)
   app.UseRequestResponseLog();
   // 用戶訪問記錄(必須放到外層，不然如果遇到異常，會報錯，因為不能返回流)(註意開啟權限，不然本地無法寫入)
   app.UseRecordAccessLogsMildd();
   // 記錄ip請求 (註意開啟權限，不然本地無法寫入)
   app.UseIpLogMildd();
   // 紀錄API慢執行時間
   app.UseStopwatchMiddleware();

   //強制顯示中文
   System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-TW");

// 只有測試環境才放swagger
#if DEBUG
    app.UseSwagger().UseSwaggerUI(c =>
   {
      //根據版本名稱倒序 遍歷展示
      typeof(CustomApiVersion.ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(
           version =>
           {
              c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"Doc {version}");
           });
      //設置默認跳轉到swagger-ui
      c.RoutePrefix = "doc";
      //c.RoutePrefix = string.Empty;
   });
#endif

   #region Hangfire定時任務

   //授權
   var filter = new BasicAuthAuthorizationFilter(
       new BasicAuthAuthorizationFilterOptions
       {
          SslRedirect = false,
          // Require secure connection for dashboard
          RequireSsl = false,
          // Case sensitive login checking
          LoginCaseSensitive = false,
          // Users
          Users = new[]
           {
            new BasicAuthAuthorizationUser
            {
                Login = AppSettingsConstVars.HangFireLogin,
                PasswordClear = AppSettingsConstVars.HangFirePassWord
            }
           }
       });
   var hangfireOptions = new Hangfire.DashboardOptions
   {
      AppPath = "/",//返回時跳轉的地址
      DisplayStorageConnectionString = false,//是否顯示資料庫連接資訊
      Authorization = new[]
       {
        filter
    },
      IsReadOnlyFunc = _ => false
   };

   //app.UseHangfireDashboard("/job", hangfireOptions); //可以改變Dashboard的url
   //HangfireDispose.HangfireService();

   #endregion

   //使用 Session
   app.UseSession();
   var isDev = app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "RemoteTW";
   if (isDev)
   {
      // 在開發環境中，使用異常頁面，這樣可以暴露錯誤堆棧資訊，所以不要放在生產環境。
      app.UseDeveloperExceptionPage();
   }
   else
   {
      app.UseExceptionHandler("/Home/Error");
      // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      app.UseHsts();
   }

   // 鏈路追蹤
   app.UseCorrelationId();

   // CORS跨域
   app.UseCors(AppSettingsConstVars.CorsPolicyName);
   // 實現http context
   app.UseStaticHttpContext();

   // Routing
   app.UseRouting();

   // 使用靜態檔案
   app.UseStaticFiles();
   // 先開啟認證
   app.UseAuthentication();
   // 然後是授權中間件
   app.UseAuthorization();
   // 分析效能
   app.UseMiniProfiler();

   // Stripe 相關
   var stripeConfig = AppSettingsHelper.GetSection<List<GeneralPaymentConfig>>("Payment", "PaymentOptions").Where(x => x.PaymentCode == "Stripe").FirstOrDefault();
   StripeConfiguration.ApiKey = stripeConfig.SecretKey;

   app.MapControllerRoute(
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}");

   //設置默認起始頁（如default.html）
   //此處的路徑是相對於wwwroot檔案夾的相對路徑
   var defaultFilesOptions = new DefaultFilesOptions();
   defaultFilesOptions.DefaultFileNames.Clear();
   
   if (isDev)
   {
      defaultFilesOptions.DefaultFileNames.Add("index.html");
   }

   app.UseDefaultFiles(defaultFilesOptions);
   app.UseStaticFiles();

   //確保NLog.config中連接字串與appsettings.json中同步

   //其他項目啟動時需要做的事情
   NLogUtil.WriteSEQLog("app start success", NLog.LogLevel.Info);
   app.Run();

}
catch (Exception exception)
{
   //使用Nlog寫到本地日誌檔案（萬一資料庫沒創建/連接成功）
   NLogUtil.WriteSEQLog("app start fail", NLog.LogLevel.Error, exception);

   throw;
}
finally
{

   // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
   NLog.LogManager.Shutdown();
}



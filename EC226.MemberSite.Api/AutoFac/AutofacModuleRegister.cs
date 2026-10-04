using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EC226.Configuration;
using EC226.Core.AOP;
using EC226.Utility.Extensions;
using Microsoft.AspNetCore.Mvc;
using NLog;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Services;
using EC226.Core.Config;

using Microsoft.Extensions.Configuration;
using EC226.Repository.UnitOfWork;
using Autofac.Core;
using EC226.MemberSite.IRepository.UnitOfWork;

namespace EC226.MemberSite.Api.AutoFac
{
   public class AutofacModuleRegister: Autofac.Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         var basePath = AppContext.BaseDirectory;

         #region 帶有介面層的服務注入

         var servicesDllFile = Path.Combine(basePath, "EC226.MemberSite.Services.dll");
         var repositoryDllFile = Path.Combine(basePath, "EC226.MemberSite.Repository.dll");

         if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
         {
            var msg = "Repository.dll和Services.dll 丟失，因為專案解耦了，所以需要先F6編譯，再F5執行，請檢查 bin 資料夾，並拷貝。";
            throw new Exception(msg);
         }

         // AOP 開關，如果想要打開指定的功能，只需要在 appsettigns.json 對應對應 true 就行。
         //var cacheType = new List<Type>();
         //if (AppSettingsConstVars.RedisConfigEnabled)
         //{
         //    builder.RegisterType<RedisCacheAop>();
         //    cacheType.Add(typeof(RedisCacheAop));
         //}
         //else
         //{
         //    builder.RegisterType<MemoryCacheAop>();
         //    cacheType.Add(typeof(MemoryCacheAop));
         //}

         // 取得 Service.dll 程式集服務，並註冊
         var assemblysServices = Assembly.LoadFrom(servicesDllFile);
         //支援屬性注入依賴重複
         builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces().InstancePerDependency()
             .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

         // 取得 Repository.dll 程式集服務，並註冊
         var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
         //支援屬性注入依賴重複
         builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces().InstancePerDependency()
             .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);


         // 取得 Service.dll 程式集服務，並註冊
         //var assemblysServices = Assembly.LoadFrom(servicesDllFile);
         //builder.RegisterAssemblyTypes(assemblysServices)
         //    .AsImplementedInterfaces()
         //    .InstancePerDependency()
         //    .PropertiesAutowired()
         //    .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
         //    .InterceptedBy(cacheType.ToArray());//允許將攔截器服務的列表分配給註冊。

         //var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
         //builder.RegisterAssemblyTypes(assemblysRepository)
         //    .AsImplementedInterfaces()
         //    .PropertiesAutowired()
         //    .InstancePerDependency();

         #endregion

         #region dynamic inject named payment option service                                                                            
         Assembly assembly = Assembly.Load("EC226.MemberSite.Services");

         // 註冊 PaymentProviderFactory
         builder.RegisterType<EC226.Payment.Factory.PaymentProviderFactory>()
            .AsSelf()
            .SingleInstance();

         // 註冊金流服務 (Named)
         builder.RegisterType<EC226.MemberSite.Services.ECPayService>()
            .Named<IThirdPaymentService>("ECPAY")
            .PropertiesAutowired()
            .InstancePerDependency();

         builder.RegisterType<EC226.MemberSite.Services.NewebpayService>()
            .Named<IThirdPaymentService>("Newebpay")
            .PropertiesAutowired()
            .InstancePerDependency();

         // ECPay 物流暫用 ECPayService
         builder.RegisterType<EC226.MemberSite.Services.ECPayService>()
            .Named<IThirdPaymentService>("ECPAY_Logistics")
            .PropertiesAutowired()
            .InstancePerDependency();

         //var generalPaymentConfigs = AppSettingsHelper.GetSection<List<GeneralPaymentConfig>>("Payment", "PaymentOptions").Where(x => !x.Disable).ToList();

         //foreach (var item in generalPaymentConfigs)
         //{
         //   var serviceTypeName = item.PaymentCode;
         //   Type serviceType = assembly.GetType($"EC226.MemberSite.Services.{serviceTypeName}Service");

         //   if (serviceType == null)
         //   {
         //      throw new Exception($"Payment {serviceType} service not found");
         //   }

         //   builder.RegisterType(serviceType)
         //      .WithProperty("GeneralPaymentConfig", item)
         //      .WithParameter(new ResolvedParameter(
         //      (pi, ctx) => pi.ParameterType == typeof(IUnitOfWork),
         //      (pi, ctx) => ctx.Resolve<IUnitOfWork>()))

         //      .PropertiesAutowired() // 啟用屬性自動注入               
         //      .Named<IPaymentOptionService>(item.PaymentCode);
         //}
         #endregion

         #region dynamic inject named insert order service 
         //List<string> insertOrderNamings = new List<string>() { "InsertTempOrderService", "InsertOrderService" };

         //foreach (var item in insertOrderNamings)
         //{
         //   Type serviceType = assembly.GetType($"EC226.MemberSite.Services.{item}");
         //   builder.RegisterType(serviceType)
         //         .PropertiesAutowired() // 啟用屬性自動注入               
         //         .Named<IInsertOrderService>(item);
         //}
         #endregion

      }
   }
}

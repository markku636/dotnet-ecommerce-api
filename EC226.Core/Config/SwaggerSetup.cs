using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EC226.Loging;
using EC226.Swagger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EC226.Core.Config
{
   public static class SwaggerSetup
   {

      public static void AddAdminSwaggerSetup(this IServiceCollection services)
      {
         if (services == null) throw new ArgumentNullException(nameof(services));
         var apiName = "";

         services.AddSwaggerGen((s) =>
         {
            s.SchemaFilter<EnumSchemaFilter>();

            //遍歷出全部的版本，做文檔資訊展示
            typeof(CustomApiVersion.ApiVersions).GetEnumNames().ToList().ForEach(version =>
               {
                  s.SwaggerDoc(version, new OpenApiInfo
                  {
                     Version = version,
                     Title = $"{apiName} API Document",
                     Description = $"{apiName} HTTP API " + version,
                  });

                  s.OrderActionsBy(o => o.RelativePath);
               });

            try
            {
               //產生API XML文檔
               var basePath = AppContext.BaseDirectory;
               var xmlPath = Path.Combine(basePath, "doc.xml");
               s.IncludeXmlComments(xmlPath);
            }
            catch (Exception ex)
            {
               NLogUtil.WriteSEQLog("Swagger產生失敗，Doc.xml丟失，請檢查並拷貝。", NLog.LogLevel.Error, ex);
            }

            // 開啟加權小鎖
            s.OperationFilter<AddResponseHeadersFilter>();
            s.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();


            // 在header中新增token，傳遞到後臺
            s.OperationFilter<SecurityRequirementsOperationFilter>();

            // 必須是 oauth2
            s.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
               Description = "JWT授權(數據將在請求頭中進行傳輸) 直接在下框中輸入Bearer {token}（註意兩者之間是一個空格）\"",
               Name = "Authorization",//jwt默認的參數名稱
               In = ParameterLocation.Header,//jwt默認存放Authorization資訊的位置(請求頭中)
               Type = SecuritySchemeType.ApiKey
            });
         });
      }


      public static void AddClientSwaggerSetup(this IServiceCollection services)
      {
         if (services == null) throw new ArgumentNullException(nameof(services));
         var apiName = "DE.Api.Startup";

         services.AddSwaggerGenNewtonsoftSupport();

         services.AddSwaggerGen((s) =>
         {
            s.SchemaFilter<EnumSchemaFilter>();
            //遍歷出全部的版本，做文檔資訊展示
            typeof(CustomApiVersion.ApiVersions).GetEnumNames().ToList().ForEach(version =>
               {
                  s.SwaggerDoc(version, new OpenApiInfo
                  {
                     Version = version,
                     Title = $"{apiName} 介面文檔",
                     Description = $"{apiName} HTTP API " + version,
                     Contact = new OpenApiContact { Name = apiName, Email = "JianWeie@163.com", Url = new Uri("https://EC226") },
                  });
                  s.OrderActionsBy(o => o.RelativePath);
               });

            try
            {
               //產生API XML文檔
               var basePath = AppContext.BaseDirectory;
               var xmlPath = Path.Combine(basePath, "doc.xml");
               s.IncludeXmlComments(xmlPath);
            }
            catch (Exception ex)
            {
               NLogUtil.WriteSEQLog("Swagger產生失敗，Doc.xml丟失，請檢查並拷貝", NLog.LogLevel.Error, ex);
            }

            // 開啟加權小鎖
            s.OperationFilter<AddResponseHeadersFilter>();
            s.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

            // 在header中新增token，傳遞到後臺
            s.OperationFilter<SecurityRequirementsOperationFilter>();

            // 必須是 oauth2
            s.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
               Description = "JWT授權(數據將在請求頭中進行傳輸) 直接在下框中輸入Bearer {token}（註意兩者之間是一個空格）\"",
               Name = "Authorization",//jwt默認的參數名稱
               In = ParameterLocation.Header,//jwt默認存放Authorization資訊的位置(請求頭中)
               Type = SecuritySchemeType.ApiKey
            });

         });
      }

   }

   public class EnumSchemaFilter: ISchemaFilter
   {
      public void Apply(OpenApiSchema model, SchemaFilterContext context)
      {
         if (context.Type.IsEnum)
         {
            StringBuilder stringBuilder = new StringBuilder();
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(name =>
                {
                   Enum e = (Enum)Enum.Parse(context.Type, name);
                   var data = $"{name}({e.GetEnumDesc()})={Convert.ToInt64(Enum.Parse(context.Type, name))}";

                   stringBuilder.AppendLine(data);
                });
            model.Description = stringBuilder.ToString();


            model.Type = context.Type.Name;
            model.Format = context.Type.Name;
         }
      }

   }

   public static class EnumExtensions
   {
      /// <summary>
      /// 获取枚举信息(枚举名称、描述、值)
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public static string GetEnumDesc(this Enum value)
      {
         var type = value.GetType();
         var names = Enum.GetNames(type).ToList();

         FieldInfo[] fields = type.GetFields();
         foreach (FieldInfo item in fields)
         {
            if (!names.Contains(item.Name))
            {
               continue;
            }
            if (value.ToString() != item.Name)
            {
               continue;
            }
            DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])item.
        GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (EnumAttributes.Length > 0)
            {
               return EnumAttributes[0].Description;
            }
            else
            {
               return "";
            }
         }

         return "";
      }
   }


}

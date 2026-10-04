
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace EC226.Configuration
{
   /// <summary>
   /// ��ȡAppsettings������Ϣ
   /// </summary>
   public class AppSettingsHelper
   {
      static IConfiguration Configuration { get; set; }

      public AppSettingsHelper(string contentPath)
      {
         // Retrieve the environment name from the 'ASPNETCORE_ENVIRONMENT' environment variable.
         var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
         // Determine the name of the settings file based on the environment.
         string fileName = $"appsettings.{environmentName}.json";
         string filePath = Path.Combine(contentPath, fileName);

         // Fallback to 'appsettings.json' if the specific environment file does not exist.
         if (!File.Exists(filePath))
         {
            fileName = "appsettings.json";
         }

         Configuration = new ConfigurationBuilder()
             .SetBasePath(contentPath)
             .AddJsonFile(fileName, optional: false, reloadOnChange: true)
             .AddEnvironmentVariables() // 允許以環境變數覆寫機敏設定，例如 JwtConfig__SecretKey
             .Build();


      }

      /// <summary>
      /// ��װҪ�������ַ�
      /// AppSettingsHelper.GetContent(new string[] { "JwtConfig", "SecretKey" });
      /// </summary>
      /// <param name="sections">�ڵ�����</param>
      /// <returns></returns>
      public static string GetContent(params string[] sections)
      {
         try
         {
            if (sections.Any())
            {
               return Configuration[string.Join(":", sections)];
            }
         }
         catch (Exception) { }

         return "";
      }

      public static T GetSection<T>(params string[] sections) where T : new()
      {
         try
         {
            T config = new T();
            if (sections.Any())
            {
               Configuration.GetSection(string.Join(":", sections)).Bind(config);

               return config;
            }
         }
         catch (Exception ex)
         { }

         return default(T);
      }

      public static async Task<T> GetSectionAsync<T>(params string[] sections) where T : new()
      {
         try
         {
            T config = new T();
            if (sections.Any())
            {
               Configuration.GetSection(string.Join(":", sections)).Bind(config);
               return await Task.FromResult(config);
            }
         }
         catch (Exception ex)
         {
            // Ideally, you should handle the exception or log it
            // Depending on your error handling policy
         }

         return default(T);
      }

   }
}

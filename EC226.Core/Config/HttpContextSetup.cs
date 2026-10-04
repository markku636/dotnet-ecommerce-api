using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EC226.Core.Config
{
   public static class HttpContext
   {
      private static IHttpContextAccessor _accessor;

      public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

      internal static void Configure(IHttpContextAccessor accessor)
      {
         _accessor = accessor;
      }  
   }
   public static class StaticHttpContextExtensions
   {
      //  HttpContextSetup 拿去執行了，所以不需要
      public static void AddHttpContextAccessor(this IServiceCollection services)
      {
         services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      }

      public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
      {
         var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
         HttpContext.Configure(httpContextAccessor);
         EC226.Utility.Helper.IpHelper.HttpContextAccessor = httpContextAccessor;
         return app;
      }
   }

   public static class CoreHttpContext
   {
      private static Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnviroment;
      public static string WebPath => _hostEnviroment.WebRootPath;

      public static string MapPath(string path)
      {
         return Path.Combine(_hostEnviroment.WebRootPath, path);
      }

      internal static void Configure(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnviroment)
      {
         _hostEnviroment = hostEnviroment;
      }
   }
   public static class StaticHostEnviromentExtensions
   {
      public static IApplicationBuilder UseStaticHostEnviroment(this IApplicationBuilder app)
      {
         var webHostEnvironment = app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
         CoreHttpContext.Configure(webHostEnvironment);
         return app;
      }
   }


}

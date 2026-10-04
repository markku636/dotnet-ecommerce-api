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


namespace EC226.Core.AutoFac
{
    public class AutofacModuleRegister : Autofac.Module
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

         //// 取得 Repository.dll 程式集服務，並註冊
         //var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
         //builder.RegisterAssemblyTypes(assemblysRepository)
         //    .AsImplementedInterfaces()
         //    .PropertiesAutowired()
         //    .InstancePerDependency();

         #endregion


         //builder.RegisterType<NewebpayService>().Named<IThirdPaymentService>("Newebpay");
                  
         

      }
    }
}

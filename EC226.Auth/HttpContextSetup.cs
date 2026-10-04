
using System;
using System.Collections.Generic;
using System.Text;
using EC226.Auth.HttpContextUser;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EC226.Auth
{
    /// <summary>
    /// 上下文啟動
    /// </summary>
    public static class HttpContextSetup
    {
        public static void AddHttpContextSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IHttpContextUser, AspNetUser>();
        }
    }
}


using System;
using EC226.Configuration;
using EC226.Utility.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace EC226.Core.Config
{
    /// <summary>
    /// 配置跨域（CORS）
    /// </summary>
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddCors(c =>
            {
                if (!AppSettingsConstVars.CorsEnableAllIPs)
                {
                    c.AddPolicy(AppSettingsConstVars.CorsPolicyName, policy =>
                        {
                            policy.WithOrigins(AppSettingsConstVars.CorsIPs.Split(','));
                            policy.AllowAnyHeader();//Ensures that the policy allows any header.
                            policy.AllowAnyMethod();
                            policy.AllowCredentials();
                        });
                }
                else
                {
                    //允許任意跨域請求
                    c.AddPolicy(AppSettingsConstVars.CorsPolicyName, policy =>
                        {
                            policy.SetIsOriginAllowed((host) => true)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                        });
                }
            });
        }
    }
}

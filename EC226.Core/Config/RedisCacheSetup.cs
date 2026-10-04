using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC226.Caching.AutoMate.RedisCache;
using EC226.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace EC226.Core.Config
{
    /// <summary>
    /// Redis快取 啟動服務
    /// </summary>
    public static class RedisCacheSetup
    {
        public static void AddRedisCacheSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<IRedisOperationRepository, RedisOperationRepository>();

            // 配置啟動Redis服務，雖然可能影響專案啟動速度，但是不能在執行的時候報錯，所以是合理的
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                //取得連線字串
                string redisConfiguration = AppSettingsConstVars.RedisConfigConnectionString;

                var configuration = ConfigurationOptions.Parse(redisConfiguration, true);

                configuration.ResolveDns = true;

                return ConnectionMultiplexer.Connect(configuration);
            });

            // Register Redis database instance
            services.AddScoped<IDatabase>(sp => 
            {
                var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                return connectionMultiplexer.GetDatabase();
            });

        }
    }
}

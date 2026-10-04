using System;
using System.Transactions;
using EC226.Configuration;
using EC226.Utility.Extensions;
using Hangfire;
using Hangfire.MySql;
using Hangfire.Redis;
using Hangfire.Redis.StackExchange;
using Hangfire.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using StackExchange.Redis;

namespace EC226.Core.Config
{
    /// <summary>
    /// 增加HangFire到單獨配置
    /// </summary>
    public static class HangFireSetup
    {
        private static ConnectionMultiplexer _redis;


        public static void AddHangFireSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //註冊Hangfire定時任務
            var isEnabledRedis = AppSettingsConstVars.RedisUseTimedTask;
            if (isEnabledRedis)
            {
                var configuration = ConfigurationOptions.Parse(AppSettingsConstVars.RedisConfigConnectionString, true);
                _redis = ConnectionMultiplexer.Connect(configuration);
                var db = _redis.GetDatabase();

                services.AddHangfire(x => x.UseRedisStorage(_redis, new RedisStorageOptions()
                {
                    Db = db.Database, //建議根據
                    SucceededListSize = 500,//後續列表中的最大可見後臺作業，以防止它無限增長。
                    DeletedListSize = 500,//刪除列表中的最大可見後臺作業，以防止其無限增長。
                    InvisibilityTimeout = TimeSpan.FromMinutes(30),
                }));
            }
            else
            {
                string dbTypeString = AppSettingsConstVars.DbDbType;
                if (dbTypeString == DbType.MySql.ToString())
                {
                    services.AddHangfire(x => x.UseStorage(new MySqlStorage(AppSettingsConstVars.DbSqlConnection, new MySqlStorageOptions
                    {
                        TransactionIsolationLevel = IsolationLevel.ReadCommitted, // 事務隔離級別。預設是讀取已提交。
                        QueuePollInterval = TimeSpan.FromSeconds(15),             //- 作業佇列輪詢間隔。預設值為15秒。
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- 作業到期檢查間隔（管理過期記錄）。預設值為1小時。
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- 聚合計數器的間隔。預設為5分鐘。
                        PrepareSchemaIfNecessary = true,                          //- 如果設定為true，則建立資料庫表。預設是true。
                        DashboardJobListLimit = 500,                            //- 儀表板作業列表限制。預設值為50000。
                        TransactionTimeout = TimeSpan.FromMinutes(1),             //- 交易超時。預設為1分鐘。
                        TablesPrefix = "Hangfire"                                  //- 資料庫中表的字首。預設為none
                    })));
                }
                else if (dbTypeString == DbType.SqlServer.ToString())
                {
                    services.AddHangfire(x => x.UseSqlServerStorage(AppSettingsConstVars.DbSqlConnection, new SqlServerStorageOptions()
                    {
                        QueuePollInterval = TimeSpan.FromSeconds(15),             //- 作業佇列輪詢間隔。預設值為15秒。
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),       //- 作業到期檢查間隔（管理過期記錄）。預設值為1小時。
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),      //- 聚合計數器的間隔。預設為5分鐘。
                        PrepareSchemaIfNecessary = true,                          //- 如果設定為true，則建立資料庫表。預設是true。
                        DashboardJobListLimit = 500,                            //- 儀表板作業列表限制。預設值為50000。
                        TransactionTimeout = TimeSpan.FromMinutes(1),             //- 交易超時。預設為1分鐘。
                    }));
                }
            }

            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { GlobalEnumVars.HangFireQueuesConfig.@default.ToString(), GlobalEnumVars.HangFireQueuesConfig.apis.ToString(), GlobalEnumVars.HangFireQueuesConfig.web.ToString(), GlobalEnumVars.HangFireQueuesConfig.recurring.ToString() };
                options.ServerTimeout = TimeSpan.FromMinutes(4);
                options.SchedulePollingInterval = TimeSpan.FromSeconds(15);//秒級任務需要配置短點，一般任務可以配置預設時間，預設15秒
                options.ShutdownTimeout = TimeSpan.FromMinutes(5); //超時時間
                options.WorkerCount = Math.Max(Environment.ProcessorCount, 20); //工作執行緒數，目前允許的最大執行緒，預設20
            });

        }
    }
}

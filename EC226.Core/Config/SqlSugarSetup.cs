using System;
using EC226.Caching.SqlSugar;
using EC226.Configuration;
using EC226.Loging;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using SqlSugar.IOC;

namespace EC226.Core.Config
{
    /// <summary>
    /// SqlSugar 啟動服務
    /// </summary>
    public static class SqlSugarSetup
    {

        public static void AddSqlSugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

         //注入 ORM
            services.AddSqlSugar(new IocConfig()
            {
               ConfigId = "master2",
               //資料庫連線
               ConnectionString = AppSettingsConstVars.DbSqlConnection,
                //判斷資料庫型別
                DbType = AppSettingsConstVars.DbDbType == IocDbType.MySql.ToString() ? IocDbType.MySql : IocDbType.SqlServer,
                //是否開啟自動關閉資料庫連線-//不設成true要手動close
                IsAutoCloseConnection = true,
            });

            //設定參數
            services.ConfigurationSugar(db =>
            {
                db.CurrentConnectionConfig.InitKeyType = InitKeyType.Attribute;
                //db.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices()
                //{
                //    //判斷是否開啟redis設定二級快取方式
                //    DataInfoCacheService = AppSettingsConstVars.RedisUseCache ? (ICacheService)new SqlSugarRedisCache() : new SqlSugarMemoryCache()
                //};

                //執行SQL 錯誤事件，可監控sql（暫時遮蔽，需要可開啟）
                //db.Aop.OnLogExecuting = (sql, p) =>
                //{
                //    NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Other, "SqlSugar執行SQL錯誤事件列印Sql", sql);
                //};

                //執行SQL 錯誤事件
                db.Aop.OnError = (exp) =>
                {
                    NLogUtil.WriteSEQLog("執行SQL錯誤事件",NLog.LogLevel.Error);
                };

                //設定更多連線參數
                //db.CurrentConnectionConfig.XXXX=XXXX
                //db.CurrentConnectionConfig.MoreSetting=new MoreSetting(){}
                //讀寫分離等都在這兒設定
            });

        }
    }
}

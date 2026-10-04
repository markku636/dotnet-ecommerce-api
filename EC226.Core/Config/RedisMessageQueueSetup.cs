using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EC226.Configuration;
using EC226.RedisMQ.Subscribe;
using EC226.Utility.Extensions;
using InitQ;
using Microsoft.Extensions.DependencyInjection;

namespace EC226.Core.Config
{
    /// <summary>
    /// Redis 訊息佇列 啟動服務
    /// </summary>
    public static class RedisMessageQueueSetup
    {
        public static void AddRedisMessageQueueSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddInitQ(m =>
            {
                //時間間隔
                m.SuspendTime = 1000;
                //redis伺服器地址
                m.ConnectionString = AppSettingsConstVars.RedisConfigConnectionString;
                //對應的訂閱者類，需要new一個實例對象，當然你也可以傳參，比如日誌對像
                m.ListSubscribe = new List<Type>() {
                    //typeof(OrderAgentOrDistributionSubscribe),
                    //typeof(OrderAutomaticDeliverySubscribe),
                    //typeof(OrderFinishCommandSubscribe),
                    //typeof(OrderPrintSubscribe),

                    //typeof(LogingSubscribe),

                    //typeof(UserSubscribe),
                    //typeof(WeChatPayNoticeSubscribe),                    
                    //typeof(AfterSalesReviewSubscribe),
                };
                //顯示日誌
                m.ShowLog = false;
            });
        }
    }
}


using System;
using Hangfire;

namespace EC226.Task
{
    public class HangfireDispose
    {
        #region 配置服務

        public static void HangfireService()
        {
            //Fire - And - forget（發佈 / 訂閱）
            //這是一個主要的後臺任務型別，持久化訊息佇列會去處理這個任務。當你建立了一個發佈 / 訂閱任務，該任務會被儲存到預設佇列裡面（預設佇列是"Default"，但是支援使用多佇列）。多個專注的工作者（Worker）會監聽這個佇列，並且從中取得任務並且完成任務。
            //BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget"));

            //延遲
            //如果想要延遲某些任務的執行，可以是用以下任務。在給定延遲時間后，任務會被排入佇列，並且和發佈 / 訂閱任務一樣執行。
            //BackgroundJob.Schedule(() => Console.WriteLine("Delayed"), TimeSpan.FromDays(1));

            //循環
            //按照週期性（小時，天等）來呼叫方法，請使用RecurringJob類。在複雜的場景，您可以使用CRON表達式指定計劃時間來處理任務。
            //RecurringJob.AddOrUpdate(() => Console.WriteLine("Daily Job"), Cron.Daily);

            //連續
            //連續性允許您通過將多個後臺任務鏈接在一起來定義複雜的工作流。
            //var id = BackgroundJob.Enqueue(() => Console.WriteLine("Hello, "));
            //BackgroundJob.ContinueWith(id, () => Console.WriteLine("world!"));

            //這裡呢就是需要觸發的方法 "0/10 * * * * ? " 可以自行搜索cron表達式 代表循環的規律很簡單
            //CancelOrderJob代表你要觸發的類 Execute代表你要觸發的方法


            ////自動取消訂單任務
            //RecurringJob.AddOrUpdate<AutoCancelOrderJob>(s => s.Execute(), "0 0/5 * * * ? ", TimeZoneInfo.Local); // 每5分鐘取消一次訂單

            ////自動完成訂單任務
            //RecurringJob.AddOrUpdate<CompleteOrderJob>(s => s.Execute(), "0 0 0/1 * * ? ", TimeZoneInfo.Local); // 每小時自動完成訂單

            ////自動評價訂單任務
            //RecurringJob.AddOrUpdate<EvaluateOrderJob>(s => s.Execute(), "0 0 0/1 * * ? ", TimeZoneInfo.Local); // 每小時自動完成訂單

            ////自動簽收訂單任務
            //RecurringJob.AddOrUpdate<AutoSignOrderJob>(s => s.Execute(), "0 0 0/1 * * ? ", TimeZoneInfo.Local); // 每小時自動完成訂單

            ////催付款訂單
            //RecurringJob.AddOrUpdate<RemindOrderPayJob>(s => s.Execute(), "0 0/5 * * * ? ", TimeZoneInfo.Local); // 每5分鐘催付款訂單

            ////拼團自動取消到期團（每分鐘執行一次）
            //RecurringJob.AddOrUpdate<AutoCanclePinTuanJob>(s => s.Execute(), "0 0/2 * * * ? ", TimeZoneInfo.Local); // 每分鐘取消一次訂單

            ////每天凌晨5點定期清理7天前操作日誌
            //RecurringJob.AddOrUpdate<RemoveOperationLogJob>(s => s.Execute(), "0 0 5 * * ? ", TimeZoneInfo.Local); // 每天5點固定時間清理一次
            
        }

        #endregion
    }
}

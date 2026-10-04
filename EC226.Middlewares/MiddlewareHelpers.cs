 using EC226.Core;
using Microsoft.AspNetCore.Builder;

namespace EC226.Middlewares
{
    /// <summary>
    /// 中介軟體
    /// </summary>
    public static class MiddlewareHelpers
    {
        /// <summary>
        /// 請求響應中介軟體
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestResponseLog(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequRespLogMildd>();
        }


        /// <summary>
        /// 異常處理中介軟體（客戶端）
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandlerMiddForClent(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddForClent>();
        }


        /// <summary>
        /// IP請求中介軟體
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIpLogMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<IPLogMildd>();
        }

        /// <summary>
        /// 使用者訪問介面日誌中介軟體
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRecordAccessLogsMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RecordAccessLogsMildd>();
        }

        /// <summary>
        /// 紀錄API慢執行時間
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStopwatchMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<StopwatchMiddleware>();
        }

    }
}

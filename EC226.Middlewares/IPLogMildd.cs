using System;
using System.Threading.Tasks;
using EC226.Configuration;
using EC226.Loging;
using EC226.MemberSite.Model.ViewModels.LogInfo;
using EC226.Utility.Extensions;
using EC226.Utility.Helper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EC226.Middlewares
{
    /// <summary>
    /// 中介軟體
    /// 記錄IP請求數據
    /// </summary>
    public class IPLogMildd
    {
        /// <summary>
        ///
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        public IPLogMildd(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (AppSettingsConstVars.MiddlewareIpLogEnabled)
            {
                // 過濾，只有介面
                if (context.Request.Path.Value != null && (context.Request.Path.Value.Contains("api") || context.Request.Path.Value.Contains("Api")))
                {
                    context.Request.EnableBuffering();
                    try
                    {
                        // 儲存請求數據
                        var dt = DateTime.Now;
                        var request = context.Request;
                        var requestInfo = JsonConvert.SerializeObject(new RequestInfo()
                        {
                            Ip = IpHelper.GetIp(),
                            Url = request.Path.ObjectToString().TrimEnd('/').ToLower(),
                            Datetime = dt.ToString("yyyy-MM-dd HH:mm:ss"),
                            Date = dt.ToString("yyyy-MM-dd"),
                            Week = CommonHelper.GetWeek(),
                        });

                        if (!string.IsNullOrEmpty(requestInfo))
                        {
                            Parallel.For(0, 1, e =>
                            {
                                LogLockHelper.OutSql2Log("RequestIpInfoLog", "RequestIpInfoLog" + dt.ToString("yyyy-MM-dd-HH"), new string[] { requestInfo + "," }, false);
                            });
                            request.Body.Position = 0;
                        }
                        await _next(context);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }

       

    }
}

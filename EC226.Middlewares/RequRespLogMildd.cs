using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EC226.Configuration;
using EC226.Loging;
using EC226.Utility.Extensions;
using Microsoft.AspNetCore.Http;

namespace EC226.Middlewares
{
    /// <summary>
    /// 中介軟體
    /// 記錄請求和響應數據
    /// </summary>
    public class RequRespLogMildd
    {
        /// <summary>
        ///
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        ///
        /// </summary>
        /// <param name="next"></param>
        public RequRespLogMildd(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (AppSettingsConstVars.MiddlewareRequestResponseLogEnabled)
            {
                // 過濾，只有介面
                if (context.Request.Path.Value.Contains("api") || context.Request.Path.Value.Contains("Api"))
                {
                    context.Request.EnableBuffering();
                    Stream originalBody = context.Response.Body;
                    try
                    {
                        // 儲存請求數據
                        await RequestDataLog(context);

                        using (var ms = new MemoryStream())
                        {
                            context.Response.Body = ms;

                            await _next(context);

                            // 儲存響應數據
                            ResponseDataLog(context.Response, ms);

                            ms.Position = 0;
                            await ms.CopyToAsync(originalBody);
                        }
                    }
                    catch (Exception)
                    {
                        // 記錄異常
                        //ErrorLogData(context.Response, ex);
                    }
                    finally
                    {
                        context.Response.Body = originalBody;
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

        private async Task RequestDataLog(HttpContext context)
        {
            var request = context.Request;
            var sr = new StreamReader(request.Body);

            var content = $" QueryData:{request.Path + request.QueryString}\r\n BodyData:{await sr.ReadToEndAsync()}";

            if (!string.IsNullOrEmpty(content))
            {
                Parallel.For(0, 1, e =>
                {
                    LogLockHelper.OutSql2Log("RequestResponseLog", "RequestResponseLog" + DateTime.Now.ToString("yyyy-MM-dd-HH"), new string[] { "Request Data:", content });

                });

                request.Body.Position = 0;
            }
        }

        private void ResponseDataLog(HttpResponse response, MemoryStream ms)
        {
            ms.Position = 0;
            var ResponseBody = new StreamReader(ms).ReadToEnd();
            // 去除 Html
            var reg = "<[^>]+>";
            var isHtml = Regex.IsMatch(ResponseBody, reg);
            if (!string.IsNullOrEmpty(ResponseBody))
            {
                Parallel.For(0, 1, e =>
                {
                    LogLockHelper.OutSql2Log("RequestResponseLog", "RequestResponseLog" + DateTime.Now.ToString("yyyy-MM-dd-HH"), new string[] { "Response Data:", ResponseBody });
                });
            }
        }
    }
}

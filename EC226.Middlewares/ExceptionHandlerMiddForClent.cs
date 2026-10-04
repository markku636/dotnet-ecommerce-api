using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EC226.Loging;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace EC226.Middlewares
{
    /// <summary>
    /// 異常錯誤統一返回記錄
    /// </summary>
    public class ExceptionHandlerMiddForClent
    {
        private readonly RequestDelegate _next;


        public ExceptionHandlerMiddForClent(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (ex == null) return;

            NLogUtil.WriteAll("全域性捕獲異常", NLog.LogLevel.Error, ex);

            await WriteExceptionAsync(context, ex).ConfigureAwait(false);
        }

        private static async Task WriteExceptionAsync(HttpContext context, Exception e)
        {
            if (e is UnauthorizedAccessException)
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else if (e is Exception)
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            context.Response.ContentType = "application/json";
            var result = new Result("全域性數據異常");

            await context.Response.WriteAsync(JsonConvert.SerializeObject(result)).ConfigureAwait(false);
        }
    }
}

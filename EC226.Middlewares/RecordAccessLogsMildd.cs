
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EC226.Auth.HttpContextUser;
using EC226.Configuration;
using EC226.Loging;
using EC226.Utility.Extensions;
using EC226.Utility.Helper;

namespace EC226.Middlewares
{
   /// <summary>
   /// 中介軟體
   /// 記錄使用者方訪問數據
   /// </summary>
   public class RecordAccessLogsMildd
   {
      /// <summary>
      ///
      /// </summary>
      private readonly RequestDelegate _next;
      private readonly IHttpContextUser _user;

      private readonly ILogger<RecordAccessLogsMildd> _logger;
      private readonly Stopwatch _stopwatch;

      /// <summary>
      /// 構造
      /// </summary>
      /// <param name="next"></param>
      /// <param name="user"></param>
      /// <param name="logger"></param>
      public RecordAccessLogsMildd(RequestDelegate next, IHttpContextUser user, ILogger<RecordAccessLogsMildd> logger)
      {
         _next = next;
         _user = user;
         _logger = logger;
         _stopwatch = new Stopwatch();
      }

      public async Task InvokeAsync(HttpContext context)
      {
         var ip = IpHelper.GetIp();

         context.Items["IP"] = ip;

         if (AppSettingsConstVars.MiddlewareRecordAccessLogsEnabled)
         {
            var api = context.Request.Path.ObjectToString().TrimEnd('/').ToLower();
            var ignoreApis = AppSettingsConstVars.MiddlewareRecordAccessLogsIgnoreApis;

            // 過濾，只有介面
            if (api.Contains("api") && !ignoreApis.Contains(api))
            {
               _stopwatch.Restart();
               var userAccessModel = new UserAccessModel();

               HttpRequest request = context.Request;

               userAccessModel.API = api;
               userAccessModel.User = _user.Name;
               userAccessModel.IP = ip;
               userAccessModel.BeginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
               userAccessModel.RequestMethod = request.Method;
               userAccessModel.Agent = request.Headers["User-Agent"].ObjectToString();


               // 取得請求body內容
               if (request.Method.ToLower().Equals("post") || request.Method.ToLower().Equals("put"))
               {
                  // 啟用倒帶功能，就可以讓 Request.Body 可以再次讀取
                  request.EnableBuffering();

                  Stream stream = request.Body;
                  byte[] buffer = new byte[request.ContentLength.Value];
                  stream.Read(buffer, 0, buffer.Length);
                  userAccessModel.RequestData = Encoding.UTF8.GetString(buffer);

                  request.Body.Position = 0;
               }
               else if (request.Method.ToLower().Equals("get") || request.Method.ToLower().Equals("delete"))
               {
                  userAccessModel.RequestData = HttpUtility.UrlDecode(request.QueryString.ObjectToString(), Encoding.UTF8);
               }

               // 取得Response.Body內容
               var originalBodyStream = context.Response.Body;
               using (var responseBody = new MemoryStream())
               {
                  context.Response.Body = responseBody;

                  await _next(context);

                  var responseBodyData = await GetResponse(context.Response);

                  await responseBody.CopyToAsync(originalBodyStream);
               }


               var dt = DateTime.Now;

               // 響應完成記錄時間和存入日誌
               context.Response.OnCompleted(() =>
               {
                  _stopwatch.Stop();

                  userAccessModel.OPTime = _stopwatch.ElapsedMilliseconds + "ms";

                  // 自定義log輸出
                  var requestInfo = JsonConvert.SerializeObject(userAccessModel);
                  Parallel.For(0, 1, e =>
                       {
                      LogLockHelper.OutSql2Log("RecordAccessLogs", "RecordAccessLogs" + dt.ToString("yyyy-MM-dd-HH"), new string[] { requestInfo + "," }, false);
                   });

                  return Task.CompletedTask;
               });

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


      /// <summary>
      /// 取得響應內容
      /// </summary>
      /// <param name="response"></param>
      /// <returns></returns>
      public async Task<string> GetResponse(HttpResponse response)
      {
         response.Body.Seek(0, SeekOrigin.Begin);
         var text = await new StreamReader(response.Body).ReadToEndAsync();
         response.Body.Seek(0, SeekOrigin.Begin);
         return text;
      }
   }

   public class UserAccessModel
   {
      public string User { get; set; }
      public string IP { get; set; }
      public string API { get; set; }
      public string BeginTime { get; set; }
      public string OPTime { get; set; }
      public string RequestMethod { get; set; }
      public string RequestData { get; set; }
      public string Agent { get; set; }

   }

}


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using EC226.MemberSite.Model.ViewModels.UI;
using EC226.Utility.Extensions;
using Microsoft.AspNetCore.Http;
using EC226.Loging;
using NPOI.SS.Formula.Functions;
using EC226.Utility.Helper;

namespace EC226.Core.Attribute
{
   public class RateLimitAttribute: ActionFilterAttribute
   {
      private static ConcurrentDictionary<string, (int Count, DateTime Expiry)> _requests = new ConcurrentDictionary<string, (int, DateTime)>();

      public int RequestLimit { get; set; }
      public int Seconds { get; set; }  // 時間窗口，單位為分鐘


      public override void OnActionExecuting(ActionExecutingContext context)
      {
         //CleanupExpiredRequests();  // 清理過期的請求記錄

         //var ipAddress = IpHelper.GetIp();
         //if (ipAddress == "-" || string.IsNullOrWhiteSpace(ipAddress))
         //{
         //   base.OnActionExecuting(context);
         //   return;
         //}

         //var now = DateTime.UtcNow;
         //var requestInfo = _requests.GetOrAdd(ipAddress, _ => (0, now.AddSeconds(Seconds)));

         //if (now > requestInfo.Expiry)
         //{
         //   requestInfo = (0, now.AddMinutes(Seconds));
         //}

         //if (requestInfo.Count >= RequestLimit)
         //{
         //   var result = new Result("系統返回異常，請聯繫管理員進行處理！");
         //   result.Code = ApiReturnCode.TooManyRequests;  // 假設這個代碼與業務邏輯相關
         //   result.Message = $"[RateLimitAttribute]{context.ActionDescriptor.DisplayName},IP:{ipAddress},Count:{requestInfo.Count},Limit:{RequestLimit}";
         //   NLogUtil.WriteSEQLog($"[RateLimitAttribute]{context.ActionDescriptor.DisplayName},IP:{ipAddress},Count:{requestInfo.Count},Limit:{RequestLimit}", NLog.LogLevel.Warn);

         //   // 將 ObjectResult 的 StatusCode 設置為 200
         //   context.Result = new ObjectResult(result)
         //   {
         //      StatusCode = StatusCodes.Status200OK  // 確保返回的 HTTP 狀態碼是 200
         //   };

         //   return;
         //}

         //_requests[ipAddress] = (requestInfo.Count + 1, requestInfo.Expiry);
         base.OnActionExecuting(context);
      }

      private void CleanupExpiredRequests()
      {
         var now = DateTime.UtcNow;
         var keysToRemove = _requests.Keys.Where(key => _requests[key].Expiry < now).ToList();

         foreach (var key in keysToRemove)
         {
            _requests.TryRemove(key, out _);
         }
      }
   }
}

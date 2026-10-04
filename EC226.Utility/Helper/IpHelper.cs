using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EC226.Utility.Helper
{
   public static class IpHelper
   {
      public static IHttpContextAccessor HttpContextAccessor { get; set; }

      // todo 暫時不支援IPv6 因為資料庫欄位長度問題，之後有空在改
      public static string GetIp(bool isHiddenIPV6 = true)
      {
         var noneIP = "-";
         try
         {
            var context = HttpContextAccessor?.HttpContext;
            if (context == null)
            {
               return noneIP;
            }

            // Get the 'X-Forwarded-For' header and split safely
            var xForwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            var ip = string.Empty;

            if (!string.IsNullOrEmpty(xForwardedFor))
            {
               var ips = xForwardedFor.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
               ip = ips.FirstOrDefault()?.Trim();
            }

            // Fallback to remote IP address if necessary
            if (string.IsNullOrEmpty(ip))
            {
               ip = context.Connection.RemoteIpAddress?.ToString();
            }

            if (ip == "::1")
            {
               // Try to get the IPv4 loopback address as a fallback
               ip = "127.0.0.1";
            }

            // Handle IPv6 visibility based on the provided flag
            if (isHiddenIPV6  && ip?.Contains(":") == true)
            {
               return noneIP; // Return a placeholder or empty string for hidden IPv6
            }

            return ip ?? noneIP;
         }
         catch
         {
            // Return a specific error message or just "error"
            return noneIP;
         }
      }
   }

}

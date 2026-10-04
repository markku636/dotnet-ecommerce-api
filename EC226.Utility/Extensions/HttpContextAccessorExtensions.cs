using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EC226.Utility.Extensions
{
   public static class HttpContextAccessorExtensions
   {
      public static string GetIpAddress(this HttpContext httpContext)
      {
         // 尝试从 Cloudflare 的头部获取 IP 地址 cloudflare tunnel 適商
         if (!string.IsNullOrEmpty(httpContext.Request.Headers["CF-CONNECTING-IP"]))
         {
            return httpContext.Request.Headers["CF-CONNECTING-IP"];
         } 

         // 尝试从 HTTP_X_FORWARDED_FOR 头部获取 IP 地址
         var forwardedIp = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
         if (!string.IsNullOrEmpty(forwardedIp))
         {
            var addresses = forwardedIp.Split(',');
            // 返回最后一个非空地址，考虑到逆向代理的情况
            return addresses.Last().Trim();
         }

         // 作为后备，使用连接的远程 IP 地址
         return httpContext.Connection.RemoteIpAddress?.ToString() ?? "IP 地址未知";
      }
   }
}

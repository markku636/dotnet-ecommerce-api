using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EC226.Middlewares
{
   public class RateLimitingMiddleware
   {
      private readonly RequestDelegate _next;
      private static ConcurrentDictionary<string, (int Count, DateTime Expiry)> _requests =
          new ConcurrentDictionary<string, (int, DateTime)>();

      private readonly int _requestLimit;
      private readonly TimeSpan _timeSpan;

      public RateLimitingMiddleware(RequestDelegate next, int requestLimit, TimeSpan timeSpan)
      {
         _next = next;
         _requestLimit = requestLimit;
         _timeSpan = timeSpan;
      }

      public async Task Invoke(HttpContext context)
      {
         var ipAddress = context.Connection.RemoteIpAddress?.ToString();

         if (ipAddress == null)
         {
            await _next(context);
            return;
         }

         var now = DateTime.UtcNow;
         var requestInfo = _requests.GetOrAdd(ipAddress, _ => (0, now.Add(_timeSpan)));

         if (now > requestInfo.Expiry)
         {
            requestInfo = (0, now.Add(_timeSpan));
         }

         if (requestInfo.Count >= _requestLimit)
         {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return;
         }

         _requests[ipAddress] = (requestInfo.Count + 1, requestInfo.Expiry);
         await _next(context);
      }
   }
}

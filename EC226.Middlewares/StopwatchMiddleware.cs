using EC226.Loging;
using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EC226.Middlewares
{
    public class StopwatchMiddleware
    {
        private readonly RequestDelegate _next;

        public StopwatchMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            long startTime = Stopwatch.GetTimestamp();
            var maxSecond = 3;

            // var id = Guid.NewGuid();
            await this._next.Invoke(context);
            TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);

            if (context?.Request?.Method != null & context?.Request?.Path != null & elapsedTime.Seconds > maxSecond)
            {
                NLogUtil.WriteSEQLog($"[ExecutionTimeApiAttribute][Slow] {context.Request.Method} {context.Request.Path} took longer than {maxSecond} seconds. Actual seconds: {elapsedTime.Seconds}", LogLevel.Info);
            }
        }
    }
}

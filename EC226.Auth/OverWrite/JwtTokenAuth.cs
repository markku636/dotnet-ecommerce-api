
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EC226.Auth.OverWrite
{
    /// <summary>
    /// 中介軟體
    /// 原做為自定義授權中介軟體
    /// 先做檢查 header token的使用
    /// </summary>
    public class JwtTokenAuth
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public JwtTokenAuth(RequestDelegate next)
        {
            _next = next;
        }


        private void PreProceed(HttpContext next)
        {
            //Console.WriteLine($"{DateTime.Now} middleware invoke preproceed");
            //...
        }
        private void PostProceed(HttpContext next)
        {
            //Console.WriteLine($"{DateTime.Now} middleware invoke postproceed");
            //....
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {
            PreProceed(httpContext);

            //檢測是否包含'Authorization'請求頭
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                PostProceed(httpContext);

                return _next(httpContext);
            }
            //var tokenHeader = httpContext.Request.Headers["Authorization"].ToString();
            var tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            try
            {
                if (tokenHeader.Length >= 128)
                {
                    //Console.WriteLine($"{DateTime.Now} token :{tokenHeader}");
                    TokenModelJwt tm = JwtHelper.SerializeJwt(tokenHeader);
                    //授權
                    //var claimList = new List<Claim>();
                    //var claim = new Claim(ClaimTypes.Role, tm.Role);
                    //claimList.Add(claim);
                    //var identity = new ClaimsIdentity(claimList);
                    //var principal = new ClaimsPrincipal(identity);
                    //httpContext.User = principal;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{DateTime.Now} middleware wrong:{e.Message}");
            }

            PostProceed(httpContext);
            return _next(httpContext);
        }

    }
}

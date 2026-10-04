using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EC226.Auth.Policys;
using EC226.Configuration;
using EC226.Utility.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using static RedisMessageQueueKey;

namespace EC226.Auth
{
    /// <summary>
    /// Db 啟動服務
    /// </summary>
    public static class AuthorizationSetup
    {
        /// <summary>
        /// 前端客戶jwt初始化設定
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthorizationSetupForClient(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            #region 參數
            //讀取配置檔案
            var symmetricKeyAsBase64 = AppSettingsConstVars.JwtConfigSecretKey;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var issuer = AppSettingsConstVars.JwtConfigIssuer;
            var audience = AppSettingsConstVars.JwtConfigAudience;

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 如果要資料庫動態繫結，這裡先留個空，後邊處理器里動態賦值
            var permission = new List<PermissionItem>();

            // 角色與介面的許可權要求參數
            var permissionRequirement = new PermissionRequirement(
                "/api/denied",// 拒絕授權的跳轉地址（目前無用）
                permission,
                ClaimTypes.Role,//基於角色的授權
                issuer,//發行人
                audience,//聽眾
                signingCredentials,//簽名憑據
                expiration: TimeSpan.FromSeconds(60 * 60 * 24)//介面的過期時間
                );
            #endregion

            // 複雜的策略授權
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.Name, policy => policy.Requirements.Add(permissionRequirement));
            });


            // 令牌驗證參數
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, //是否驗證SecurityKey
                IssuerSigningKey = signingKey, //拿到SecurityKey
                ValidateIssuer = true, //是否驗證Issuer
                ValidIssuer = issuer,//發行人
                ValidateAudience = true, //是否驗證Audience
                ValidAudience = audience,//訂閱人
                ValidateLifetime = true, //是否驗證失效時間
                ClockSkew = TimeSpan.FromSeconds(60),
                RequireExpirationTime = true,
            };

            // core自帶官方JWT認證，開啟Bearer認證
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = nameof(ApiResponseForClientHandler);
                o.DefaultForbidScheme = nameof(ApiResponseForClientHandler);
            })
             // 新增JwtBearer服務
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnChallenge = context =>
                     {
                         context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                         return Task.CompletedTask;
                     },
                     OnAuthenticationFailed = context =>
                     {
                         var token = context.Request.Headers["Authorization"].ObjectToString().Replace("Bearer ", "");
                         var jwtToken = (new JwtSecurityTokenHandler()).ReadJwtToken(token);

                         if (jwtToken.Issuer != issuer)
                         {
                             context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                         }

                         if (jwtToken.Audiences.FirstOrDefault() != audience)
                         {
                             context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                         }

                         // 如果過期，則把<是否過期>新增到，返回頭資訊中
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };
             })
             .AddScheme<AuthenticationSchemeOptions, ApiResponseForClientHandler>(nameof(ApiResponseForClientHandler), o => { });


            // 注入許可權處理器
            services.AddScoped<IAuthorizationHandler, PermissionForClientHandler>();
            services.AddSingleton(permissionRequirement);
        }


    }
}

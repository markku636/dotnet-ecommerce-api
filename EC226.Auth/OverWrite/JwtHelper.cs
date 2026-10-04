
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using EC226.Configuration;
using EC226.Utility.Extensions;
using Microsoft.IdentityModel.Tokens;
using NetTaste;

namespace EC226.Auth.OverWrite
{
    public class JwtHelper
    {

        /// <summary>
        /// 頒發JWT字串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModelJwt tokenModel) {
            string iss = AppSettingsConstVars.JwtConfigIssuer;
            string aud = AppSettingsConstVars.JwtConfigAudience;
            string secret = AppSettingsConstVars.JwtConfigSecretKey;

            //var claims = new Claim[] //old
            var claims = new List<Claim>
                {
                 /*
                 * 特別重要：
                   1、這裡將使用者的部分資訊，比如 uid 存到了Claim 中，如果你想知道如何在其他地方將這個 uid從 Token 中取出來，請看下邊的SerializeJwt() 方法，或者在整個解決方案，搜索這個方法，看哪裡使用了！
                   2、你也可以研究下 HttpContext.User.Claims ，具體的你可以看看 Policys/PermissionHandler.cs 類中是如何使用的。
                 */

                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                //這個就是過期時間，可自定義，注意JWT有自己的緩衝過期時間
                // Ref: https://stackoverflow.com/questions/39728519/jwtsecuritytoken-doesnt-expire-when-it-should
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddHours(12)).ToUnixTimeSeconds()}"),
                new Claim(ClaimTypes.Expiration, DateTime.Now.AddHours(12).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,aud),
                new Claim(ClaimTypes.Email, tokenModel.Email),
                
                //new Claim(ClaimTypes.Role,tokenModel.Role),//爲了解決一個使用者多個角色(比如：Admin,System)，用下邊的方法
               };

            // 可以將一個使用者的多個角色全部賦予；
            // 作者：DX 提供技術支援；
            //claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

            //秘鑰 (SymmetricSecurityKey 對安全性的要求，金鑰的長度太短會報出異常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: iss,
                claims: claims,
                signingCredentials: creds);

            // Add custom payload data
            jwt.Payload["MemID"] = tokenModel.MemID;

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr) {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

            var tm = new TokenModelJwt {
                MemID = (int)(long)jwtToken.Payload["MemID"],
                Email = jwtToken.Payload[ClaimTypes.Email].ToString()
            };
            return tm;
        }

        /// <summary>
        /// 获取过期时间
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static DateTime GetExp(string jwtStr) {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

            DateTime expDate = (jwtToken.Payload[JwtRegisteredClaimNames.Exp] ?? 0).GetInt().GetTimeSpmpToDate();
            return expDate;
        }

        public static bool IsExp(string jwtStr) {
            return GetExp(jwtStr) < DateTime.Now;
        }
    }

    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJwt
    {
        public int MemID { get; set; }

        public string Email { get; set; }
    }
}

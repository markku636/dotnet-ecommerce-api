using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EC226.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EC226.Auth
{
    public class TokenHelper
    {
        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>
        private static readonly string Secret = AppSettingsHelper.GetContent("JwtConfig", "TokenHelperSecret"); // Base64，請於 appsettings / 環境變數設定
        public static string GenerateToken(string username, int expireMinutes = 120)
        { // 此方法用來產生 Token
            var symmetricKey = Convert.FromBase64String(Secret);  // 產生二進制位元組陣列
            var tokenHandler = new JwtSecurityTokenHandler(); // 建立一個JwtSecurityTokenHandler類用來產生Token
            var now = DateTime.UtcNow; // 取得目前時間
            var tokenDescriptor = new SecurityTokenDescriptor // 建立一個 Token 的原始對像
            {
                Subject = new ClaimsIdentity(new[] // Token的身份證，類似一個人可以有身份證，戶口本
                        {
                            new Claim(ClaimTypes.Name, username) // 可以建立多個
                        }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)), // Token 有效期

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256)
                // 產生一個Token證書，第一個參數是根據預先的二進制位元組陣列產生一個安全秘鑰，說白了就是密碼，第二個參數是編碼方式
            };
            var stoken = tokenHandler.CreateToken(tokenDescriptor); // 產生一個編碼后的token對像實例
            var token = tokenHandler.WriteToken(stoken); // 產生token字串，給前端使用
            return token;
        }
        public static ClaimsPrincipal GetPrincipal(string token)
        { // 此方法用解碼字串token，並返回秘鑰的資訊對像
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler(); // 建立一個JwtSecurityTokenHandler類，用來後續操作
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken; // 將字串token解碼成token對像
                if (jwtToken == null)
                    return null;
                var symmetricKey = Convert.FromBase64String(Secret); // 產生編碼對應的位元組陣列
                var validationParameters = new TokenValidationParameters() // 產生驗證token的參數
                {
                    RequireExpirationTime = true, // token是否包含有效期
                    ValidateIssuer = false, // 驗證秘鑰發行人，如果要驗證在這裡指定發行人字串即可
                    ValidateAudience = false, // 驗證秘鑰的接受人，如果要驗證在這裡提供接收人字串即可
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey) // 產生token時的安全秘鑰
                };
                SecurityToken securityToken; // 接受解碼后的token對像
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                return principal; // 返回秘鑰的主體對象，包含秘鑰的所有相關資訊
            }

            catch
            {
                return null;
            }
        }



        /// <summary>
        /// 此方法用解碼字串token，並返回秘鑰的資訊對像
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static int GetUserIdBySecurityToken(string token)
        {
            //讀取配置檔案
            var symmetricKeyAsBase64 = AppSettingsConstVars.JwtConfigSecretKey;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var issuer = AppSettingsConstVars.JwtConfigIssuer;
            var audience = AppSettingsConstVars.JwtConfigAudience;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler(); // 建立一個JwtSecurityTokenHandler類，用來後續操作
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken; // 將字串token解碼成token對像
                if (jwtToken == null)
                    return 0;
                var validationParameters = new TokenValidationParameters() // 產生驗證token的參數
                {
                    ValidateIssuerSigningKey = true,   //是否驗證SecurityKey
                    IssuerSigningKey = signingKey,  //拿到SecurityKey
                    ValidateIssuer = true, //是否驗證Issuer
                    ValidIssuer = issuer,//發行人 //Issuer，這兩項和前面簽發jwt的設定一致
                    ValidateAudience = true, //是否驗證Audience
                    ValidAudience = audience,//訂閱人
                    ValidateLifetime = true,//是否驗證失效時間
                    ClockSkew = TimeSpan.FromSeconds(60),
                    RequireExpirationTime = true,
                };
                SecurityToken securityToken; // 接受解碼后的token對像
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                if (securityToken == null || string.IsNullOrEmpty(securityToken.Id))
                {
                    return 0;
                }
                return Convert.ToInt32(securityToken.Id); // 返回秘鑰的主體對象，包含秘鑰的所有相關資訊
            }

            catch
            {
                return 0;
            }
        }


    }
}

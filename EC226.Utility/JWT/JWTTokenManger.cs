using EC226.Utility.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EC226.Utility.JWT
{
    public class JWTTokenManger<T> where T : class, new()
    {
        private readonly ILogger<JWTTokenManger<T>> _log;
        private readonly IHttpContextAccessor _httpContextAccessor;


        //public JWTTokenManger(ILogger<JWTTokenManger<T>> log, IHttpContextAccessor httpContextAccessor)
        //{
        //    _log = log;
        //    _httpContextAccessor = httpContextAccessor;

        //}

        // Server Key (要抽到config)
        private string key = "ZZZZZZZ-YYYYYYY-CCCCCCCCCC-DDDDDDDDDD-EEEEEEEEEE-FFFFFFFFFF-GGGGGGGGGG";

        public JWTToken Generator(T userData, int accessTopkenExpiredMintues = 1440)
        {
            try
            {
                JWTHeader jwtHeader = new JWTHeader()
                {
                    ALG = "HS256",
                    Type = "JWT",
                    IV = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16)
                };

                var headerJson = JsonConvert.SerializeObject(jwtHeader);

                var base64Header = Convert.ToBase64String(Encoding.UTF8.GetBytes(headerJson));

                var payload = new Payload<T>
                {
                    Data = userData,
                    ExpiredDate = Convert.ToInt32((DateTime.UtcNow.AddMinutes(accessTopkenExpiredMintues).Subtract(new DateTime(1970, 1, 1)).TotalSeconds))
                };

                var payloadJson = JsonConvert.SerializeObject(payload);

                var base64Payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));

                var encrypt = TokenCryptoHelper.AESEncrypt(base64Payload, key.Substring(0, 16), jwtHeader.IV);

                var signature = TokenCryptoHelper.ComputeHMACSHA256(jwtHeader.IV + "." + encrypt, key.Substring(0, 64));

                JWTToken jwtToken = new JWTToken
                {
                    //Token 為 iv + encrypt + signature，並用 . 串聯
                    AccessToken = base64Header + "." + encrypt + "." + signature,
                    RefreshToken = Guid.NewGuid().ToString().Replace("-", ""),
                    Expired = accessTopkenExpiredMintues * 60,
                };

                return jwtToken;
            }
            catch (Exception e)
            {
                _log.LogError($"[JWTTokenManger][GetLogger]{e.Message}");
                return null;
            }
        }


        public (bool IsValidate, Payload<T> Payload) Validate(string jwttoken, string ip)
        {
            if (string.IsNullOrWhiteSpace(jwttoken))
            {
                _log.LogError($"[JWTTokenManger][Validate]JWT Token is Null");
                return (false, null);
            }

            var payload = GetPayload(jwttoken);

            var result = payload != null;

            if (!result)
            {
                _log.LogError($"[JWTTokenManger][Validate]JWT Token({jwttoken}) is Invalid!");
            }

            return (result, payload);
        }

        public Payload<T> GetPayload(string jwttoken)
        {
            try
            {
                var split = jwttoken.Split('.');
                var headerJson = Encoding.UTF8.GetString(Convert.FromBase64String(split[0]));
                var jwtHeader = JsonConvert.DeserializeObject<JWTHeader>(headerJson);

                var encrypt = split[1];
                var signature = split[2];

                if (signature != TokenCryptoHelper.ComputeHMACSHA256(jwtHeader.IV + "." + encrypt, key.Substring(0, 64)))
                { return null; }

                var base64 = TokenCryptoHelper.AESDecrypt(encrypt, key.Substring(0, 16), jwtHeader.IV);
                var payLoadjson = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                var payload = JsonConvert.DeserializeObject<Payload<T>>(payLoadjson);


                if (payload.IsExpired)
                {
                    _log.LogError($"[JWTTokenManger][IsExpired]{payload}");
                    return null;

                }

                return payload;
            }
            catch (Exception e)
            {
                _log.LogError($"[JWTTokenManger][GetPayload]{e.Message}");
                return null;
            }
        }

        public string GetJWTTokenFromHeader()
        {

            string token = string.Empty;
            var request = _httpContextAccessor.HttpContext.Request;

            if (request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = request.Headers["Authorization"];

                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                if (authHeaderVal.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase) &&
                    authHeaderVal.Parameter != null)
                {
                    token = authHeaderVal.Parameter;
                }
            }
            return token;
        }
    }
}

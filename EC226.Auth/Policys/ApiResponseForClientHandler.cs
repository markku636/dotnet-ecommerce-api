using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EC226.Utility.Extensions;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EC226.Auth.Policys
{
    public class ApiResponseForClientHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public ApiResponseForClientHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            //Response.StatusCode = StatusCodes.Status401Unauthorized;
            //await Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse(StatusCode.CODE401)));

            var jsonSettings = JsonSettingsExtend.GetDefaultSettings();
            var result = new Result();

            if (Response.Headers["Token-Expired"] == "true") {
                result.Fail("Token Expired", ApiReturnCode.Access_Token_Expired);
            } else {
                result.Fail("Unauthorized", ApiReturnCode.Invalid_Access_Token);
            }

            await Response.WriteAsync(JsonConvert.SerializeObject(result, jsonSettings));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            //Response.StatusCode = StatusCodes.Status403Forbidden;
            //await Response.WriteAsync(JsonConvert.SerializeObject(new ApiResponse(StatusCode.CODE403)));

            var jsonSettings = JsonSettingsExtend.GetDefaultSettings();
            var result = new Result();
            result.Fail("很抱歉，授權失效，請重新登錄!", ApiReturnCode.Access_Token_Expired);

            await Response.WriteAsync(JsonConvert.SerializeObject(result, jsonSettings));

        }

    }
}

using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using EC226.Configuration;
using EC226.Utility.Extensions;
using EC226.MemberSite.Model.ViewModels.UI;
using System.Net.Http.Headers;

namespace EC226.Utility.Helper
{
    public class CloudflareHelper
    {
        /// <summary>
        /// Validate Cloudflare Turnstile token to prevent requests from robots
        /// </summary>
        public static bool ValidateCloudflareTurnstileToken(string methodNameForErrorLog, string turnstileToken, string bypassToken = "") {
            // For local development and UI Tests to bypass validation
            if (bypassToken == AppSettingsConstVars.CloudflareTurnstileBypassToken) {
                return true;
            }

            Guard.Against<ErrorExtensions.TurnstileException>(String.IsNullOrEmpty(turnstileToken), "Empty Token");

            using (var httpClient = new HttpClient()) {
                var rawDataObject = new {
                    secret = AppSettingsConstVars.CloudflareTurnstileSecretKey,
                    response = turnstileToken,
                    remoteip = "" // TODO!! add ip
                };
                string jsonContent = JsonConvert.SerializeObject(rawDataObject);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = httpClient.PostAsync("https://challenges.cloudflare.com/turnstile/v0/siteverify", content);

                var result = response.Result.Content.ReadAsStringAsync();
                var success = result.Result.Contains("\"success\":true");

                Guard.Against<ErrorExtensions.TurnstileException>(!success, $"Error Response: {result.Result}");

                return true;
            }

        }
    }
}

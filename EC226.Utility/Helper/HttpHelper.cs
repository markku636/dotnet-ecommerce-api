
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EC226.MemberSite.Model.ViewModels.UI;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using SqlSugar;

namespace EC226.Utility.Helper
{
   /// <summary>
   /// 模擬標準表單Post提交
   /// </summary>
   public static class HttpHelper
   {
      public static async Task<string> GetAsync(string url, Dictionary<string, string> headers)
      {
         try
         {
            using (var httpClient = new HttpClient())
            {
               using (var request = new HttpRequestMessage(HttpMethod.Get, url))
               {
                  // Add headers from the dictionary to the request
                  foreach (var header in headers)
                  {
                     request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                  }

                  var response = await httpClient.SendAsync(request);
                  var result = await response.Content.ReadAsStringAsync();
                  if (response.IsSuccessStatusCode)
                  {
                     return result;
                  }
                  else
                  {
                     throw new Exception(result);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            throw ex;
         }
      }

      // Asynchronous GET request with expected return type T
      public static async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers)
      {
         T result = default;

         try
         {
            var jsonResult = await GetAsync(url, headers);

            if (string.IsNullOrWhiteSpace(jsonResult))
            {
               return result;
            }

            result = JsonConvert.DeserializeObject<T>(jsonResult);
         }
         catch (Exception ex)
         {
            throw ex;
         }

         return result;
      }

      public static async Task<string> PostAsync(string url, string requestBody, Dictionary<string, string> headers)
      {
         try
         {
            if (!headers.ContainsKey("Content-Type"))
            {
               headers.Add("Content-Type", "application/json");
            }

            using (var httpClient = new HttpClient())
            {
               using (var request = new HttpRequestMessage(new HttpMethod("POST"), url))
               {
                  // Add headers from the dictionary to the request
                  foreach (var header in headers)
                  {
                     request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                  }

                  request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");


                  var response = await httpClient.SendAsync(request);
                  var result = await response.Content.ReadAsStringAsync();
                  if (response.IsSuccessStatusCode)
                  {
                     return result;
                  }
                  else
                  {
                     throw new Exception(result);
                  }
               }
            }
         }
         catch (Exception ex)
         {
            throw ex;
         }
      }

      public static async Task<T> PostAsync<T>(string url, string requestBody, Dictionary<string, string> headers)
      {

         T result = default(T);

         try
         {
            var jsonResult = await PostAsync(url, requestBody, headers);

            if (string.IsNullOrWhiteSpace(jsonResult))
            {
               return result;
            }

            result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonResult);
         }
         catch (Exception ex)
         {
            throw ex;            
         }

         return result;
      }
   }
}

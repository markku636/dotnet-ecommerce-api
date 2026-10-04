using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EC226.Utility.Extensions
{
   public static class StringExtension
   {
      public static string UrlCombine(this string uri1, string uri2)
      {
         uri1 = uri1.TrimEnd('/');
         uri2 = uri2.TrimStart('/');
         return string.Format("{0}/{1}", uri1, uri2);
      }

      public static bool IsUrl(this string str)
      {
         if (string.IsNullOrEmpty(str))
            return false;
         string Url = @"(http://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
         return Regex.IsMatch(str, Url);

      }

      public static int GetInt(this object obj)
      {
         if (obj == null)
            return 0;
         int.TryParse(obj.ToString(), out int _number);
         return _number;

      }

      private static DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);

      private static long longTime = 621355968000000000;

      private static int samllTime = 10000000;

      /// <summary>
      /// 获取时间戳 
      /// </summary>
      /// <param name="dateTime"></param>
      /// <returns></returns>
      public static long GetTimeStamp(this DateTime dateTime)
      {
         return (dateTime.ToUniversalTime().Ticks - longTime) / samllTime;
      }
      /// <summary>
      /// 时间戳转换成日期
      /// </summary>
      /// <param name="timeStamp"></param>
      /// <returns></returns>
      public static DateTime GetTimeSpmpToDate(this object timeStamp)
      {
         if (timeStamp == null) return dateStart;
         DateTime dateTime = new DateTime(longTime + Convert.ToInt64(timeStamp) * samllTime, DateTimeKind.Utc).ToLocalTime();
         return dateTime;
      }

      /// <summary>
      /// HMACSHA1加密
      /// </summary>
      /// <param name="target"></param>
      /// <param name="key"></param>
      /// <returns></returns>
      public static String HMACSHA1(this string target, string key)
      {
         HMACSHA1 hmacsha1 = new HMACSHA1();
         hmacsha1.Key = Encoding.UTF8.GetBytes(key);
         byte[] dataBuffer = Encoding.UTF8.GetBytes(target);
         byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);

         return Convert.ToBase64String(hashBytes);
      }

      /// <summary>
      /// HMACSHA256加密
      /// </summary>
      /// <param name="target"></param>
      /// <param name="key"></param>
      /// <returns></returns>
      public static String HMACSHA256(string message, string key)
      {
         ASCIIEncoding encoding = new ASCIIEncoding();
         byte[] keyByte = encoding.GetBytes(key);

         HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);
         byte[] messageBytes = encoding.GetBytes(message);
         byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);

         string sha256Str = string.Empty;

         for (int i = 0; i < hashmessage.Length; i++)
         {
            sha256Str += hashmessage[i].ToString("x2");
         }

         return sha256Str;
      }

   }
}

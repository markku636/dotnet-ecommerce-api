using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar.Extensions;

namespace EC226.Configuration
{
   /// <summary>
   /// 配置檔案格式化
   /// </summary>
   public class AppSettingsConstVars
   {

      #region 全域性地址================================================================================

      /// <summary>
      /// 系統介面地址
      /// </summary>
      public static readonly string ApiUrl = AppSettingsHelper.GetContent("AppConfig", "ApiUrl");

      /// <summary>
      /// 系統介面地址
      /// </summary>
      public static readonly string FrontendUrl =
         #if DEBUG
                       "http://localhost:3001";
                       // "https://example.com"; // some payments need https 
         #else
                         AppSettingsHelper.GetContent("AppConfig", "FrontendUrl");
         #endif

      public static readonly string WebPortalUrl = AppSettingsHelper.GetContent("AppConfig", "WebPortalUrl");

      #endregion

      #region 資料庫================================================================================
      /// <summary>
      /// 取得資料庫連線字串
      /// </summary>
      public static readonly string DbSqlConnection = AppSettingsHelper.GetContent("ConnectionStrings", "SqlConnection");
      /// <summary>
      /// 取得資料庫型別
      /// </summary>
      public static readonly string DbDbType = AppSettingsHelper.GetContent("ConnectionStrings", "DbType");
      #endregion

      #region redis================================================================================

      /// <summary>
      /// 取得redis連線字串
      /// </summary>
      public static readonly string RedisConfigConnectionString = AppSettingsHelper.GetContent("RedisConfig", "ConnectionString");

      /// <summary>
      /// 啟用redis作為快取選擇
      /// </summary>
      public static readonly bool RedisUseCache = AppSettingsHelper.GetContent("RedisConfig", "UseCache").ObjToBool();
      /// <summary>
      /// 啟用redis作為定時任務
      /// </summary>
      public static readonly bool RedisUseTimedTask = AppSettingsHelper.GetContent("RedisConfig", "UseTimedTask").ObjToBool();

      #endregion

      #region AOP================================================================================
      /// <summary>
      /// 事務切面開關
      /// </summary>
      public static readonly bool TranAopEnabled = AppSettingsHelper.GetContent("TranAOP", "Enabled").ObjToBool();

      #endregion

      #region Jwt授權配置================================================================================

      public static readonly string JwtConfigSecretKey = AppSettingsHelper.GetContent("JwtConfig", "SecretKey");
      public static readonly string JwtConfigIssuer = AppSettingsHelper.GetContent("JwtConfig", "Issuer");
      public static readonly string JwtConfigAudience = AppSettingsHelper.GetContent("JwtConfig", "Audience");
      #endregion

      #region Cors跨域設定================================================================================
      public static readonly string CorsPolicyName = AppSettingsHelper.GetContent("Cors", "PolicyName");
      public static readonly bool CorsEnableAllIPs = AppSettingsHelper.GetContent("Cors", "EnableAllIPs").ObjToBool();
      public static readonly string CorsIPs = AppSettingsHelper.GetContent("Cors", "IPs");
      #endregion

      #region Middleware中介軟體================================================================================
      /// <summary>
      /// Ip限流
      /// </summary>
      public static readonly bool MiddlewareIpLogEnabled = AppSettingsHelper.GetContent("Middleware", "IPLog", "Enabled").ObjToBool();
      /// <summary>
      /// 記錄請求與返回數據
      /// </summary>
      public static readonly bool MiddlewareRequestResponseLogEnabled = AppSettingsHelper.GetContent("Middleware", "RequestResponseLog", "Enabled").ObjToBool();
      /// <summary>
      /// 使用者訪問記錄-是否開啟
      /// </summary>
      public static readonly bool MiddlewareRecordAccessLogsEnabled = AppSettingsHelper.GetContent("Middleware", "RecordAccessLogs", "Enabled").ObjToBool();
      /// <summary>
      /// 使用者訪問記錄-過濾ip
      /// </summary>
      public static readonly string MiddlewareRecordAccessLogsIgnoreApis = AppSettingsHelper.GetContent("Middleware", "RecordAccessLogs", "IgnoreApis");

      #endregion

      #region HangFire定時任務================================================================================
      /// <summary>
      /// 登錄賬號
      /// </summary>
      public static readonly string HangFireLogin = AppSettingsHelper.GetContent("HangFire", "Login");
      /// <summary>
      /// 登錄密碼
      /// </summary>
      public static readonly string HangFirePassWord = AppSettingsHelper.GetContent("HangFire", "PassWord");


      #endregion

      #region Google相關================================================================================
      public static readonly string GoogleReCaptchaKeyBackEndV2 = AppSettingsHelper.GetContent("GoogleReCaptcha", "KeyBackEndV2");
      public static readonly string GoogleReCaptchaKeyBackEndV3 = AppSettingsHelper.GetContent("GoogleReCaptcha", "KeyBackEndV3");
      #endregion

      #region Cloudflare相關================================================================================
      public static readonly string CloudflareTurnstileSecretKey =
#if DEBUG
                  "1x0000000000000000000000000000000AA";
#else
                    AppSettingsHelper.GetContent("Cloudflare", "Turnstile", "SecretKey");
#endif
      public static readonly string CloudflareTurnstileBypassToken = AppSettingsHelper.GetContent("Cloudflare", "Turnstile", "BypassToken");
      #endregion

      #region Sftp相關================================================================================
      public static readonly string SftpHost = AppSettingsHelper.GetContent("Sftp", "Host");
      public static readonly int SftpPort = Int32.Parse(AppSettingsHelper.GetContent("Sftp", "Port"));
      public static readonly string SftpUsername = AppSettingsHelper.GetContent("Sftp", "Username");
      public static readonly string SftpPassword = AppSettingsHelper.GetContent("Sftp", "Password");
      public static readonly string SftpCdnBaseUrl = AppSettingsHelper.GetContent("Sftp", "CdnBaseUrl");
      public static readonly string SftpRelativeImagePath = AppSettingsHelper.GetContent("Sftp", "RelativeImagePath");
      #endregion

      #region Urls相關================================================================================
      public static readonly string WebUrl =
#if DEBUG
              "http://localhost:3001";
#else
                AppSettingsHelper.GetContent("AppConfig", "FrontendUrl");
#endif
      #endregion

      #region Mails相關================================================================================
      public static class Mails
      {
         public static readonly string SalesEmail = AppSettingsHelper.GetContent("Mail", "SalesEmail");
         public static readonly string GuestEmail = AppSettingsHelper.GetContent("Mail", "GuestEmail");
         public static readonly string RegisterEmail = AppSettingsHelper.GetContent("Mail", "RegisterEmail");
         public static readonly string AccountEmail = AppSettingsHelper.GetContent("Mail", "Account");
         public static readonly string AccountEmailPass = AppSettingsHelper.GetContent("Mail", "Password");
         public static readonly string Host = AppSettingsHelper.GetContent("Mail", "Host");
         public static readonly string PriorityHigh = "High";

      }
      #endregion            
   }
}

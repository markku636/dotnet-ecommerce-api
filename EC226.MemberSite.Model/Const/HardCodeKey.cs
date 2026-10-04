using System.Collections.Generic;

namespace EC226.MemberSite.Model.Const;

public static class HardCodeKey
{
   #region Site Setting
   public const string BasedCurrency = "EUR";

   public const string DateFormat = "dd.MM.yyyy";

   public const string DateTimeFormat = "dd.MM.yyyy HH:mm:ss";

   public const string CountryCode = "DE";

   public const decimal GeneralEUCountryRate = 0.19m; // 歐盟國家稅率，應該是by 國家 ，為省時間，原本是在後台稅率中，但因為這樣開發比較方便，所以先寫死

   public const string CloudflareIPCookieKey = "cl_ip";

   public const int CartCacheTime = 720; // 12 hour

   public const string All = "ALL";

   public const int UnlimitedStockNumber = -99;

   public const int MaxAllowedQuantity = 3;

   public const int GearModelID = 0;

   public const int RushSkuIDExcludedFromRushFee = 326; // 這個SkuID不算在Rush Fee裡

   public const string SaveConfiguratorCartID = "save-configurator";

   public static readonly HashSet<string> EuMemberCountryCodes = new HashSet<string>
    {
        "AT", // 奧地利
        "BE", // 比利時
        "BG", // 保加利亞
        "HR", // 克羅地亞
        "CY", // 塞浦路斯
        "CZ", // 捷克
        "DK", // 丹麥
        "EE", // 愛沙尼亞
        "FI", // 芬蘭
        "FR", // 法國
        "DE", // 德國
        "GR", // 希臘
        "HU", // 匈牙利
        "IE", // 愛爾蘭
        "IT", // 義大利
        "LV", // 拉脫維亞
        "LT", // 立陶宛
        "LU", // 盧森堡
        "MT", // 馬爾他
        "NL", // 荷蘭
        "PL", // 波蘭
        "PT", // 葡萄牙
        "RO", // 羅馬尼亞
        "SK", // 斯洛伐克
        "SI", // 斯洛維尼亞
        "ES", // 西班牙
        "SE"  // 瑞典
    };

   public static readonly Dictionary<string, string> PostalCodePatterns = new()
    {
        { "AT", @"^\d{4}$" }, // 奥地利: 4位数字
        { "BE", @"^\d{4}$" }, // 比利时: 4位数字
        { "BG", @"^\d{4}$" }, // 保加利亚: 4位数字
        { "HR", @"^\d{5}$" }, // 克罗地亚: 5位数字
        { "CY", @"^\d{4}$" }, // 塞浦路斯: 4位数字
        { "CZ", @"^\d{3}\s?\d{2}$" }, // 捷克共和国: 3位数字 + 可选空格 + 2位数字
        { "DK", @"^\d{4}$" }, // 丹麦: 4位数字
        { "EE", @"^\d{5}$" }, // 爱沙尼亚: 5位数字
        { "FI", @"^\d{5}$" }, // 芬兰: 5位数字
        { "FR", @"^\d{5}$" }, // 法国: 5位数字
        { "DE", @"^\d{5}$" }, // 德国: 5位数字
        { "GR", @"^\d{3}\s?\d{2}$" }, // 希腊: 3位数字 + 可选空格 + 2位数字
        { "HU", @"^\d{4}$" }, // 匈牙利: 4位数字
        { "IE", @"^[A-Z]\d{2}\s?[A-Z0-9]{4}$" }, // 爱尔兰: 1个字母 + 2位数字 + 可选空格 + 4位字母或数字
        { "IT", @"^\d{5}$" }, // 意大利: 5位数字
        { "LV", @"^LV-\d{4}$" }, // 拉脱维亚: "LV-" 前缀 + 4位数字
        { "LT", @"^LT-\d{5}$" }, // 立陶宛: "LT-" 前缀 + 5位数字
        { "LU", @"^\d{4}$" }, // 卢森堡: 4位数字
        { "MT", @"^[A-Z]{3}\s?\d{4}$" }, // 马耳他: 3个字母 + 可选空格 + 4位数字
        { "NL", @"^\d{4}\s?[A-Z]{2}$" }, // 荷兰: 4位数字 + 可选空格 + 2个字母
        { "PL", @"^\d{2}-\d{3}$" }, // 波兰: 2位数字 + "-" + 3位数字
        { "PT", @"^\d{4}-\d{3}$" }, // 葡萄牙: 4位数字 + "-" + 3位数字
        { "RO", @"^\d{6}$" }, // 罗马尼亚: 6位数字
        { "SK", @"^\d{3}\s?\d{2}$" }, // 斯洛伐克: 3位数字 + 可选空格 + 2位数字
        { "SI", @"^\d{4}$" }, // 斯洛文尼亚: 4位数字
        { "ES", @"^\d{5}$" }, // 西班牙: 5位数字
        { "SE", @"^\d{3}\s?\d{2}$" } // 瑞典: 3位数字 + 可选空格 + 2位数字
    };
   #endregion

   #region Guest
   public static class Guest
   {
      public const string Mail = "guest@example.com";
      public const int MemID = -1;
   }
   #endregion

   #region Email Template Folder and File Names
   public static class EmailTemplateFolder
   {
      public const string DE = "de";
      public const string EN = "en";
   }

   public static class EmailTemplateFile
   {
      public const string LoginMailResetPassword = "LoginMailResetPassword";
      public const string LoginMailCreateMember = "LoginMailCreateMember";
      public const string OrderConfirmation = "OrderConfirmation";
      public const string ContactUsForm = "ContactUsForm";
      public const string SaveConfigurator = "SaveConfigurator";

   }
   #endregion

   #region Payment

   public static class Payment
   {
      public const decimal TestAmount = 1.00M;
   }


   #endregion

   #region Emails
   public static class Emails
   {
      public const string salesEmail = "sales@example.com";
      public const string supportEmail = "support@example.com";
      public const string infoEmail = "info@example.com";
   }
   #endregion

}

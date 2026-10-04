using System.ComponentModel;

namespace EC226.Configuration
{
    /// <summary>
    /// 系統常用列舉類
    /// </summary>
    public class GlobalEnumVars
    {
        #region 系統相關===============================================================

        /// <summary>
        /// 快取優先順序:低、普通、高、永不移除
        /// </summary>
        public enum CacheItemPriority
        {
            Low = 0,
            Normal = 1,
            High = 2,
            NeverRemove = 3
        }

        /// <summary>
        /// 使用者登錄方式
        /// </summary>
        public enum LoginType
        {
            [Description("普通")]
            Common = 1,
            [Description("簡訊")]
            Sms = 2,
            [Description("微信小程式拉取手機號")]
            WeChatPhoneNumber = 3,
        }

        /// <summary>
        /// 訂單編號型別大全
        /// </summary>
        public enum SerialNumberType
        {
            [Description("訂單編號")]
            訂單編號 = 1,
            [Description("支付單編號")]
            支付單編號 = 2,
            [Description("商品編號")]
            商品編號 = 3,
            [Description("貨品編號")]
            貨品編號 = 4,
            [Description("售後單編號")]
            售後單編號 = 5,
            [Description("退款單編號")]
            退款單編號 = 6,
            [Description("退貨單編號")]
            退貨單編號 = 7,
            [Description("發貨單編號")]
            發貨單編號 = 8,
            [Description("提貨單號")]
            提貨單號 = 9,
            [Description("服務訂單編號")]
            服務訂單編號 = 10,
            [Description("服務券兌換碼")]
            服務券兌換碼 = 11,
        }
        /// <summary>
        /// 來源
        /// 訂單來源[對應EC226Order表source欄位]
        /// </summary>
        public enum Source
        {
            [Description("PC頁面")]
            PC頁面 = 1,
            [Description("H5頁面")]
            H5頁面 = 2,
            [Description("微信小程式")]
            微信小程式 = 3,
            [Description("支付寶小程式")]
            支付寶小程式 = 4,
            [Description("APP")]
            APP = 5,
            [Description("頭條系小程式")]
            頭條 = 6
        }

        /// <summary>
        /// 使用者登錄日誌型別
        /// </summary>

        public enum LoginRecordType
        {
            登錄成功 = 0,
            登錄失敗 = 1,
            退出登錄 = 2,
            重新整理Token = 0
        }


        /// <summary>
        /// 附件儲存支援型別
        /// </summary>
        public enum FilesStorageOptionsType
        {
            [Description("本地儲存")]
            LocalStorage = 0,
            [Description("阿里云OSS")]
            AliYunOSS = 1,
            [Description("騰訊云COS")]
            QCloudOSS = 2,
            [Description("七牛云KoDo")]
            QiNiuKoDo = 3,
        }


        /// <summary>
        /// 本地儲存位置
        /// </summary>
        public enum FilesStorageLocation
        {
            [Description("後端")]
            Admin = 0,
            [Description("API介面端")]
            API = 1
        }

        #endregion

        /// <summary>
        /// 表單驗證型別
        /// </summary>
        public enum FormValidationTypes
        {
            [Description("string")]
            字串 = 1,
            [Description("number")]
            數字 = 2,
            [Description("integer")]
            整數 = 3,
            [Description("price")]
            價格 = 4,
            [Description("email")]
            郵箱 = 5,
            [Description("mobile")]
            手機號 = 6,
            [Description("array")]
            多數據 = 7,
        }
        public enum HangFireQueuesConfig
        {
            /// <summary>
            /// 預設
            /// </summary>
            [Description("預設")]
            @default = 1,
            /// <summary>
            /// 介面
            /// </summary>
            [Description("介面")]
            apis = 2,
            /// <summary>
            /// 網站
            /// </summary>
            [Description("網站")]
            web = 3,
            /// <summary>
            /// 循環時間
            /// </summary>
            [Description("循環時間")]
            recurring = 4,
        }                            

        #region redis快取型別
        public enum AccessTokenEnum
        {

            /// <summary>
            /// 微信小程式
            /// </summary>
            WxOpenAccessToken = 1,

            /// <summary>
            /// 微信公眾號
            /// </summary>
            WeiXinAccessToken = 2,

            /// <summary>
            /// 易聯云印表機
            /// </summary>
            YiLianYunAccessToken = 3,
        }
        #endregion

    }
}

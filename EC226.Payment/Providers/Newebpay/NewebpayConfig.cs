using EC226.Payment.Abstractions;

namespace EC226.Payment.Providers.Newebpay
{
    /// <summary>
    /// 藍新金流設定
    /// </summary>
    public class NewebpayConfig : IPaymentConfig
    {
        /// <summary>
        /// 商店代號
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// HashKey (AES 加密/SHA256 加密用)
        /// </summary>
        public string HashKey { get; set; }

        /// <summary>
        /// HashIV (AES 加密/SHA256 加密用)
        /// </summary>
        public string HashIV { get; set; }

        /// <summary>
        /// MPG 金流 API 網址
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 付款完成後端通知網址 (Server POST)
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 付款完成前端返回網址 (Client Redirect)
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 是否為測試環境
        /// </summary>
        public bool IsSandbox { get; set; }

        /// <summary>
        /// 串接程式版本
        /// </summary>
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 取號完成通知網址 (用於 ATM/CVS/BARCODE)
        /// </summary>
        public string CustomerUrl { get; set; }

        /// <summary>
        /// 支付取消返回網址
        /// </summary>
        public string ClientBackUrl { get; set; }

        /// <summary>
        /// 語系 (zh-tw/en/jp)
        /// </summary>
        public string LangType { get; set; } = "zh-tw";

        /// <summary>
        /// 是否需要登入藍新會員
        /// </summary>
        public int LoginType { get; set; } = 0;

        /// <summary>
        /// 建立測試環境設定
        /// </summary>
        public static NewebpayConfig CreateSandbox(string merchantId, string hashKey, string hashIV)
        {
            return new NewebpayConfig
            {
                MerchantID = merchantId,
                HashKey = hashKey,
                HashIV = hashIV,
                ApiUrl = "https://ccore.newebpay.com/MPG/mpg_gateway",
                IsSandbox = true,
                Version = "2.0"
            };
        }

        /// <summary>
        /// 建立正式環境設定
        /// </summary>
        public static NewebpayConfig CreateProduction(string merchantId, string hashKey, string hashIV)
        {
            return new NewebpayConfig
            {
                MerchantID = merchantId,
                HashKey = hashKey,
                HashIV = hashIV,
                ApiUrl = "https://core.newebpay.com/MPG/mpg_gateway",
                IsSandbox = false,
                Version = "2.0"
            };
        }
    }
}

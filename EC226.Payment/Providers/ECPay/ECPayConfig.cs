using EC226.Payment.Abstractions;

namespace EC226.Payment.Providers.ECPay
{
    /// <summary>
    /// 綠界金流設定
    /// </summary>
    public class ECPayConfig : IPaymentConfig
    {
        /// <summary>
        /// 商店代號/特店編號
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// HashKey
        /// </summary>
        public string HashKey { get; set; }

        /// <summary>
        /// HashIV
        /// </summary>
        public string HashIV { get; set; }

        /// <summary>
        /// 金流 API 網址
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
        /// 特店旗下店舖代號 (選填)
        /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// 是否為平台商 (選填)
        /// </summary>
        public bool IsPlatform { get; set; }

        /// <summary>
        /// 加密類型 (0: MD5, 1: SHA256)
        /// </summary>
        public int EncryptType { get; set; } = 1;

        /// <summary>
        /// 建立測試環境設定
        /// </summary>
        public static ECPayConfig CreateSandbox()
        {
            return new ECPayConfig
            {
                MerchantID = "2000132",
                HashKey = "5294y06JbISpM5x9",
                HashIV = "v77hoKGq4kWxNNIS",
                ApiUrl = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5",
                IsSandbox = true,
                EncryptType = 1
            };
        }

        /// <summary>
        /// 建立正式環境設定
        /// </summary>
        public static ECPayConfig CreateProduction(string merchantId, string hashKey, string hashIV)
        {
            return new ECPayConfig
            {
                MerchantID = merchantId,
                HashKey = hashKey,
                HashIV = hashIV,
                ApiUrl = "https://payment.ecpay.com.tw/Cashier/AioCheckOut/V5",
                IsSandbox = false,
                EncryptType = 1
            };
        }
    }
}

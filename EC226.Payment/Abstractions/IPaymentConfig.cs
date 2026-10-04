namespace EC226.Payment.Abstractions
{
    /// <summary>
    /// 統一金流設定介面
    /// </summary>
    public interface IPaymentConfig
    {
        /// <summary>
        /// 商店代號/特店編號
        /// </summary>
        string MerchantID { get; }

        /// <summary>
        /// 加密金鑰
        /// </summary>
        string HashKey { get; }

        /// <summary>
        /// 加密向量
        /// </summary>
        string HashIV { get; }

        /// <summary>
        /// 金流 API 網址
        /// </summary>
        string ApiUrl { get; }

        /// <summary>
        /// 付款完成後端通知網址 (Server POST)
        /// </summary>
        string NotifyUrl { get; }

        /// <summary>
        /// 付款完成前端返回網址 (Client Redirect)
        /// </summary>
        string ReturnUrl { get; }

        /// <summary>
        /// 是否為測試環境
        /// </summary>
        bool IsSandbox { get; }
    }
}

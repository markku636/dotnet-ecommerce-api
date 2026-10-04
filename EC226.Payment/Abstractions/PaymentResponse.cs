namespace EC226.Payment.Abstractions
{
    /// <summary>
    /// 統一付款回應模型
    /// </summary>
    public class PaymentResponse
    {
        /// <summary>
        /// 是否成功建立付款
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// HTML Form (用於 POST 至金流商)
        /// </summary>
        public string HtmlForm { get; set; }

        /// <summary>
        /// 轉導網址 (選用，若金流商支援 GET 方式)
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 金流商交易編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 原始請求資料 (JSON，供除錯用)
        /// </summary>
        public string RawRequest { get; set; }

        /// <summary>
        /// 建立成功回應
        /// </summary>
        public static PaymentResponse CreateSuccess(string htmlForm, string orderNo, string rawRequest = null)
        {
            return new PaymentResponse
            {
                Success = true,
                HtmlForm = htmlForm,
                OrderNo = orderNo,
                RawRequest = rawRequest
            };
        }

        /// <summary>
        /// 建立失敗回應
        /// </summary>
        public static PaymentResponse CreateFailure(string errorCode, string errorMessage)
        {
            return new PaymentResponse
            {
                Success = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };
        }
    }
}

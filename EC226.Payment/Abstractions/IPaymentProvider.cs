using Microsoft.AspNetCore.Http;

namespace EC226.Payment.Abstractions
{
    /// <summary>
    /// 統一金流 Provider 介面
    /// </summary>
    public interface IPaymentProvider
    {
        /// <summary>
        /// 金流類型
        /// </summary>
        PaymentProviderType ProviderType { get; }

        /// <summary>
        /// 建立付款請求，產生 HTML Form
        /// </summary>
        /// <param name="request">付款請求參數</param>
        /// <returns>付款回應 (含 HTML Form)</returns>
        PaymentResponse CreatePayment(PaymentRequest request);

        /// <summary>
        /// 驗證並解析回調通知 (From Form POST)
        /// </summary>
        /// <param name="form">表單資料</param>
        /// <returns>回調結果</returns>
        PaymentCallbackResult ValidateCallback(IFormCollection form);

        /// <summary>
        /// 驗證並解析回調通知 (From Encrypted Data)
        /// </summary>
        /// <param name="encryptedData">加密資料</param>
        /// <returns>回調結果</returns>
        PaymentCallbackResult ValidateCallback(string encryptedData);

        /// <summary>
        /// 產生回應給金流商的內容
        /// </summary>
        /// <param name="success">是否成功處理</param>
        /// <returns>回應內容字串</returns>
        string GenerateCallbackResponse(bool success);
    }

    /// <summary>
    /// 金流提供商類型
    /// </summary>
    public enum PaymentProviderType
    {
        /// <summary>
        /// 綠界科技
        /// </summary>
        ECPay,

        /// <summary>
        /// 藍新金流
        /// </summary>
        Newebpay,

        /// <summary>
        /// 綠界物流
        /// </summary>
        ECPayLogistics
    }
}

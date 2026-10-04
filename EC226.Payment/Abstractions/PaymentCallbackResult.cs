using System;

namespace EC226.Payment.Abstractions
{
    /// <summary>
    /// 統一付款回調結果模型
    /// </summary>
    public class PaymentCallbackResult
    {
        /// <summary>
        /// 驗證是否通過 (CheckMac/SHA256 驗證)
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 是否付款成功
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// 訂單編號 (商店訂單編號)
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 金流商交易編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentType { get; set; }

        /// <summary>
        /// 付款時間
        /// </summary>
        public DateTime? PaymentTime { get; set; }

        /// <summary>
        /// 交易狀態碼
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 交易狀態訊息
        /// </summary>
        public string StatusMessage { get; set; }

        /// <summary>
        /// 自訂欄位 1
        /// </summary>
        public string CustomField1 { get; set; }

        /// <summary>
        /// 自訂欄位 2
        /// </summary>
        public string CustomField2 { get; set; }

        /// <summary>
        /// 自訂欄位 3
        /// </summary>
        public string CustomField3 { get; set; }

        /// <summary>
        /// 自訂欄位 4
        /// </summary>
        public string CustomField4 { get; set; }

        /// <summary>
        /// 原始資料 (JSON 格式，供除錯/記錄用)
        /// </summary>
        public string RawData { get; set; }

        /// <summary>
        /// 應回應給金流商的內容
        /// </summary>
        public string ResponseContent { get; set; }

        #region ATM/CVS 取號資訊

        /// <summary>
        /// 銀行代碼 (ATM 用)
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 虛擬帳號/繳費代碼
        /// </summary>
        public string PaymentCode { get; set; }

        /// <summary>
        /// 繳費期限
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// 條碼 1 (超商條碼用)
        /// </summary>
        public string Barcode1 { get; set; }

        /// <summary>
        /// 條碼 2 (超商條碼用)
        /// </summary>
        public string Barcode2 { get; set; }

        /// <summary>
        /// 條碼 3 (超商條碼用)
        /// </summary>
        public string Barcode3 { get; set; }

        #endregion

        /// <summary>
        /// 建立驗證成功的結果
        /// </summary>
        public static PaymentCallbackResult CreateValid(bool isPaid, string orderNo, string tradeNo, decimal amount)
        {
            return new PaymentCallbackResult
            {
                IsValid = true,
                IsPaid = isPaid,
                OrderNo = orderNo,
                TradeNo = tradeNo,
                Amount = amount
            };
        }

        /// <summary>
        /// 建立驗證失敗的結果
        /// </summary>
        public static PaymentCallbackResult CreateInvalid(string message)
        {
            return new PaymentCallbackResult
            {
                IsValid = false,
                IsPaid = false,
                StatusMessage = message
            };
        }
    }
}

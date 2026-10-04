using System;

namespace EC226.Payment.Abstractions
{
    /// <summary>
    /// 統一金流例外
    /// </summary>
    public class PaymentException : Exception
    {
        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// 金流提供商類型
        /// </summary>
        public PaymentProviderType Provider { get; }

        public PaymentException(PaymentProviderType provider, string errorCode, string message)
            : base(message)
        {
            Provider = provider;
            ErrorCode = errorCode;
        }

        public PaymentException(PaymentProviderType provider, string errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            Provider = provider;
            ErrorCode = errorCode;
        }
    }
}

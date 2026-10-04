using System;
using EC226.Payment.Abstractions;
using EC226.Payment.Enums;
using EC226.Payment.Providers.ECPay;
using EC226.Payment.Providers.Newebpay;
using Microsoft.Extensions.Configuration;

namespace EC226.Payment.Factory
{
    /// <summary>
    /// 金流 Provider 工廠
    /// </summary>
    public class PaymentProviderFactory
    {
        private readonly IConfiguration _configuration;

        public PaymentProviderFactory(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// 依據金流類型建立對應的 Provider
        /// </summary>
        /// <param name="paymentType">金流類型</param>
        /// <returns>金流 Provider</returns>
        public IPaymentProvider Create(PaymentType paymentType)
        {
            return paymentType switch
            {
                PaymentType.ECPAY => CreateECPayProvider(),
                PaymentType.Newebpay => CreateNewebpayProvider(),
                PaymentType.ECPAY_Logistics => CreateECPayProvider(), // 物流暫用金流 Provider
                _ => throw new NotSupportedException($"不支援的金流類型: {paymentType}")
            };
        }

        /// <summary>
        /// 建立綠界 Provider
        /// </summary>
        public IPaymentProvider CreateECPayProvider()
        {
            var config = GetECPayConfig();
            return new ECPayPaymentProvider(config);
        }

        /// <summary>
        /// 建立藍新 Provider
        /// </summary>
        public IPaymentProvider CreateNewebpayProvider()
        {
            var config = GetNewebpayConfig();
            return new NewebpayPaymentProvider(config);
        }

        /// <summary>
        /// 取得綠界設定
        /// </summary>
        public ECPayConfig GetECPayConfig()
        {
            var section = _configuration.GetSection("Payment:Providers:ECPay");

            if (!section.Exists())
            {
                // 回傳預設測試環境設定
                return ECPayConfig.CreateSandbox();
            }

            return new ECPayConfig
            {
                MerchantID = section["MerchantID"],
                HashKey = section["HashKey"],
                HashIV = section["HashIV"],
                ApiUrl = section["ApiUrl"],
                NotifyUrl = section["NotifyUrl"],
                ReturnUrl = section["ReturnUrl"],
                IsSandbox = bool.TryParse(section["IsSandbox"], out var isSandbox) && isSandbox,
                StoreID = section["StoreID"],
                IsPlatform = bool.TryParse(section["IsPlatform"], out var isPlatform) && isPlatform,
                EncryptType = int.TryParse(section["EncryptType"], out var encryptType) ? encryptType : 1
            };
        }

        /// <summary>
        /// 取得藍新設定
        /// </summary>
        public NewebpayConfig GetNewebpayConfig()
        {
            var section = _configuration.GetSection("Payment:Providers:Newebpay");

            if (!section.Exists())
            {
                throw new InvalidOperationException("未設定藍新金流 (Payment:Providers:Newebpay)");
            }

            return new NewebpayConfig
            {
                MerchantID = section["MerchantID"],
                HashKey = section["HashKey"],
                HashIV = section["HashIV"],
                ApiUrl = section["ApiUrl"],
                NotifyUrl = section["NotifyUrl"],
                ReturnUrl = section["ReturnUrl"],
                CustomerUrl = section["CustomerUrl"],
                ClientBackUrl = section["ClientBackUrl"],
                IsSandbox = bool.TryParse(section["IsSandbox"], out var isSandbox) && isSandbox,
                Version = section["Version"] ?? "2.0",
                LangType = section["LangType"] ?? "zh-tw",
                LoginType = int.TryParse(section["LoginType"], out var loginType) ? loginType : 0
            };
        }
    }
}

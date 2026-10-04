using System;
using Xunit;
using FluentAssertions;
using EC226.Payment.Providers.Newebpay;
using EC226.Payment.Abstractions;

namespace EC226.Payment.UnitTests
{
    /// <summary>
    /// Newebpay 金流 Provider 測試
    /// </summary>
    public class NewebpayPaymentProviderTest
    {
        private readonly NewebpayConfig _config;
        private readonly NewebpayPaymentProvider _provider;

        public NewebpayPaymentProviderTest()
        {
            // 使用測試設定 (請填入實際的測試金鑰)
            _config = new NewebpayConfig
            {
                MerchantID = "TestMerchant",
                HashKey = "12345678901234567890123456789012", // 32 bytes
                HashIV = "1234567890123456", // 16 bytes
                ApiUrl = "https://ccore.newebpay.com/MPG/mpg_gateway",
                NotifyUrl = "https://mysite.com/api/payment/newebpay/notify",
                ReturnUrl = "https://mysite.com/payment/success",
                Version = "2.0",
                IsSandbox = true
            };

            _provider = new NewebpayPaymentProvider(_config);
        }

        [Fact]
        public void CreatePayment_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new PaymentRequest
            {
                OrderNo = "TEST001",
                Amount = 100,
                ItemDescription = "測試商品",
                BuyerEmail = "test@example.com",
                PaymentMethod = PaymentMethodType.Credit,
                TradeDate = DateTime.Now
            };

            // Act
            var response = _provider.CreatePayment(request);

            // Assert
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.HtmlForm.Should().NotBeNullOrEmpty();
            response.HtmlForm.Should().Contain("newebpay-form");
            response.HtmlForm.Should().Contain(_config.ApiUrl);
            response.HtmlForm.Should().Contain("MerchantID");
            response.HtmlForm.Should().Contain("TradeInfo");
            response.HtmlForm.Should().Contain("TradeSha");
            response.HtmlForm.Should().Contain("Version");
        }

        [Fact]
        public void CreatePayment_ATMPayment_SetsVACCParameter()
        {
            // Arrange
            var request = new PaymentRequest
            {
                OrderNo = "TEST002",
                Amount = 500,
                ItemDescription = "ATM付款測試",
                PaymentMethod = PaymentMethodType.ATM,
                TradeDate = DateTime.Now
            };

            // Act
            var response = _provider.CreatePayment(request);

            // Assert
            response.Success.Should().BeTrue();
            response.RawRequest.Should().Contain("VACC");
        }

        [Fact]
        public void CreatePayment_CVSPayment_SetsCVSParameter()
        {
            // Arrange
            var request = new PaymentRequest
            {
                OrderNo = "TEST003",
                Amount = 200,
                ItemDescription = "超商代碼測試",
                PaymentMethod = PaymentMethodType.CVS,
                TradeDate = DateTime.Now
            };

            // Act
            var response = _provider.CreatePayment(request);

            // Assert
            response.Success.Should().BeTrue();
            response.RawRequest.Should().Contain("CVS");
        }

        [Fact]
        public void CreatePayment_OrderNoTooLong_ReturnsFailure()
        {
            // Arrange
            var request = new PaymentRequest
            {
                OrderNo = "TEST001234567890123456789", // > 20 chars
                Amount = 100,
                ItemDescription = "測試商品"
            };

            // Act
            var response = _provider.CreatePayment(request);

            // Assert
            response.Success.Should().BeFalse();
            response.ErrorMessage.Should().Contain("訂單編號");
        }

        [Fact]
        public void CreatePayment_ItemDescriptionTooLong_ReturnsFailure()
        {
            // Arrange
            var request = new PaymentRequest
            {
                OrderNo = "TEST001",
                Amount = 100,
                ItemDescription = new string('A', 51) // > 50 chars
            };

            // Act
            var response = _provider.CreatePayment(request);

            // Assert
            response.Success.Should().BeFalse();
            response.ErrorMessage.Should().Contain("商品描述");
        }

        [Fact]
        public void GenerateCallbackResponse_Success_ReturnsSUCCESS()
        {
            // Act
            var response = _provider.GenerateCallbackResponse(true);

            // Assert
            response.Should().Be("SUCCESS");
        }

        [Fact]
        public void ProviderType_ShouldBeNewebpay()
        {
            // Assert
            _provider.ProviderType.Should().Be(PaymentProviderType.Newebpay);
        }
    }

    /// <summary>
    /// Newebpay 加解密測試
    /// </summary>
    public class NewebpayEncryptionTest
    {
        private const string TestHashKey = "12345678901234567890123456789012"; // 32 bytes
        private const string TestHashIV = "1234567890123456"; // 16 bytes

        [Fact]
        public void AesEncrypt_ValidInput_ReturnsHexString()
        {
            // Arrange
            var plainText = "MerchantID=TestMerchant&Amt=100";

            // Act
            var encrypted = NewebpayEncryption.AesEncrypt(plainText, TestHashKey, TestHashIV);

            // Assert
            encrypted.Should().NotBeNullOrEmpty();
            encrypted.Should().MatchRegex("^[0-9a-f]+$"); // Hex 字串
        }

        [Fact]
        public void AesDecrypt_EncryptedData_ReturnsOriginalText()
        {
            // Arrange
            var originalText = "MerchantID=TestMerchant&Amt=100&ItemDesc=TestItem";
            var encrypted = NewebpayEncryption.AesEncrypt(originalText, TestHashKey, TestHashIV);

            // Act
            var decrypted = NewebpayEncryption.AesDecrypt(encrypted, TestHashKey, TestHashIV);

            // Assert
            decrypted.Should().Be(originalText);
        }

        [Fact]
        public void GenerateTradeSha_ValidInput_ReturnsUppercaseHex()
        {
            // Arrange
            var tradeInfo = "abc123def456";

            // Act
            var tradeSha = NewebpayEncryption.GenerateTradeSha(tradeInfo, TestHashKey, TestHashIV);

            // Assert
            tradeSha.Should().NotBeNullOrEmpty();
            tradeSha.Should().MatchRegex("^[0-9A-F]+$"); // 大寫 Hex 字串
            tradeSha.Length.Should().Be(64); // SHA256 = 32 bytes = 64 hex chars
        }

        [Fact]
        public void ValidateTradeSha_CorrectSha_ReturnsTrue()
        {
            // Arrange
            var tradeInfo = "test123";
            var tradeSha = NewebpayEncryption.GenerateTradeSha(tradeInfo, TestHashKey, TestHashIV);

            // Act
            var isValid = NewebpayEncryption.ValidateTradeSha(tradeInfo, tradeSha, TestHashKey, TestHashIV);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateTradeSha_IncorrectSha_ReturnsFalse()
        {
            // Arrange
            var tradeInfo = "test123";
            var wrongSha = "WRONGSHA256VALUE1234567890ABCDEF1234567890ABCDEF1234567890ABCDEF";

            // Act
            var isValid = NewebpayEncryption.ValidateTradeSha(tradeInfo, wrongSha, TestHashKey, TestHashIV);

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void AesEncrypt_EmptyInput_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                NewebpayEncryption.AesEncrypt("", TestHashKey, TestHashIV));
        }

        [Fact]
        public void AesEncrypt_NullHashKey_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                NewebpayEncryption.AesEncrypt("test", null, TestHashIV));
        }
    }
}

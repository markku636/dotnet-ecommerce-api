using System;
using Xunit;
using FluentAssertions;
using EC226.Payment.Providers.ECPay;
using EC226.Payment.Abstractions;

namespace EC226.Payment.UnitTests
{
    /// <summary>
    /// ECPay 金流 Provider 測試
    /// </summary>
    public class ECPayPaymentProviderTest
    {
        private readonly ECPayConfig _config;
        private readonly ECPayPaymentProvider _provider;

        public ECPayPaymentProviderTest()
        {
            // 使用測試環境設定
            _config = ECPayConfig.CreateSandbox();
            _config.NotifyUrl = "https://mysite.com/api/payment/ecpay/notify";
            _config.ReturnUrl = "https://mysite.com/payment/success";

            _provider = new ECPayPaymentProvider(_config);
        }

        [Fact]
        public void CreatePayment_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new PaymentRequest
            {
                OrderNo = $"TEST{DateTime.Now:yyyyMMddHHmmss}",
                Amount = 100,
                ItemDescription = "測試商品",
                ItemName = "測試商品 100 x 1",
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
            response.HtmlForm.Should().Contain("ecpay-form");
            response.HtmlForm.Should().Contain(_config.ApiUrl);
            response.HtmlForm.Should().Contain("MerchantID");
            response.HtmlForm.Should().Contain("CheckMacValue");
        }

        [Fact]
        public void CreatePayment_AllPaymentMethod_ReturnsHtmlWithAllPayment()
        {
            // Arrange
            var request = new PaymentRequest
            {
                OrderNo = "TEST001",
                Amount = 500,
                ItemDescription = "多項商品",
                PaymentMethod = PaymentMethodType.ALL,
                TradeDate = DateTime.Now
            };

            // Act
            var response = _provider.CreatePayment(request);

            // Assert
            response.Success.Should().BeTrue();
            response.HtmlForm.Should().Contain("ChoosePayment");
            response.HtmlForm.Should().Contain("ALL");
        }

        [Fact]
        public void CreatePayment_EmptyOrderNo_ReturnsFailure()
        {
            // Arrange
            var request = new PaymentRequest
            {
                OrderNo = "",
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
        public void CreatePayment_ZeroAmount_ReturnsFailure()
        {
            // Arrange
            var request = new PaymentRequest
            {
                OrderNo = "TEST001",
                Amount = 0,
                ItemDescription = "測試商品"
            };

            // Act
            var response = _provider.CreatePayment(request);

            // Assert
            response.Success.Should().BeFalse();
            response.ErrorMessage.Should().Contain("交易金額");
        }

        [Fact]
        public void CreatePayment_WithCustomFields_IncludesCustomFieldsInForm()
        {
            // Arrange
            var request = new PaymentRequest
            {
                OrderNo = "TEST001",
                Amount = 100,
                ItemDescription = "測試商品",
                CustomField1 = "Custom1",
                CustomField2 = "Custom2",
                TradeDate = DateTime.Now
            };

            // Act
            var response = _provider.CreatePayment(request);

            // Assert
            response.Success.Should().BeTrue();
            response.HtmlForm.Should().Contain("CustomField1");
            response.HtmlForm.Should().Contain("Custom1");
            response.HtmlForm.Should().Contain("CustomField2");
            response.HtmlForm.Should().Contain("Custom2");
        }

        [Fact]
        public void GenerateCallbackResponse_Success_Returns1OK()
        {
            // Act
            var response = _provider.GenerateCallbackResponse(true);

            // Assert
            response.Should().Be("1|OK");
        }

        [Fact]
        public void GenerateCallbackResponse_Failure_Returns0ERROR()
        {
            // Act
            var response = _provider.GenerateCallbackResponse(false);

            // Assert
            response.Should().Be("0|ERROR");
        }

        [Fact]
        public void ProviderType_ShouldBeECPay()
        {
            // Assert
            _provider.ProviderType.Should().Be(PaymentProviderType.ECPay);
        }
    }
}

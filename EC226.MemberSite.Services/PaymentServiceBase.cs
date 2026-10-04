using System;
using EC226.Payment;
using EC226.Payment.Abstractions;
using EC226.Payment.Factory;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EC226.MemberSite.Services
{
    /// <summary>
    /// 金流服務基類
    /// </summary>
    public abstract class PaymentServiceBase : IThirdPaymentService
    {
        protected readonly IConfiguration _configuration;
        protected readonly PaymentProviderFactory _providerFactory;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 金流 Provider 類型 (由子類別實作)
        /// </summary>
        protected abstract PaymentProviderType ProviderType { get; }

        /// <summary>
        /// 取得 Provider (由子類別實作)
        /// </summary>
        protected abstract IPaymentProvider GetProvider();

        protected PaymentServiceBase(
            IConfiguration configuration,
            PaymentProviderFactory providerFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// 建立測試付款表單
        /// </summary>
        public virtual Result<PaymentData> TestCreatePayForm(PaymentMethod paymentMethod)
        {
            var result = new Result<PaymentData>();

            try
            {
                var provider = GetProvider();

                var request = new PaymentRequest
                {
                    OrderNo = $"TEST{DateTime.Now:yyyyMMddHHmmss}",
                    Amount = 100,
                    ItemDescription = "測試商品",
                    ItemName = "測試商品 100 x 1",
                    BuyerEmail = "test@example.com",
                    PaymentMethod = ConvertPaymentMethod(paymentMethod),
                    TradeDate = DateTime.Now
                };

                var response = provider.CreatePayment(request);

                if (response.Success)
                {
                    result.Data = new PaymentData
                    {
                        HtmlPostForm = response.HtmlForm
                    };
                    result.Success();
                }
                else
                {
                    result.Message = response.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                result.Message = $"建立測試付款表單失敗: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 建立付款表單
        /// </summary>
        public virtual Result<PaymentData> CreatePayForm(PaymentParameter paymentParameter)
        {
            var result = new Result<PaymentData>();

            try
            {
                var provider = GetProvider();

                var request = new PaymentRequest
                {
                    OrderNo = paymentParameter.OrderNo,
                    Amount = paymentParameter.Amount,
                    ItemDescription = paymentParameter.Description ?? "商品購買",
                    ItemName = paymentParameter.Description,
                    BuyerEmail = paymentParameter.ReceiverEmail,
                    BuyerName = paymentParameter.ReceiverName,
                    BuyerPhone = paymentParameter.ReceiverCellPhone,
                    PaymentMethod = ConvertPaymentMethod(paymentParameter.PaymentMethod),
                    TradeDate = paymentParameter.Date != default ? paymentParameter.Date : DateTime.Now
                };

                var response = provider.CreatePayment(request);

                if (response.Success)
                {
                    result.Data = new PaymentData
                    {
                        HtmlPostForm = response.HtmlForm
                    };
                    result.Success();
                }
                else
                {
                    result.Message = response.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                result.Message = $"建立付款表單失敗: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 付款回調通知
        /// </summary>
        public virtual Result NotifyPay()
        {
            var result = new Result();

            try
            {
                var provider = GetProvider();
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null)
                {
                    result.Message = "HttpContext 為空";
                    return result;
                }

                var callbackResult = provider.ValidateCallback(httpContext.Request.Form);

                if (!callbackResult.IsValid)
                {
                    result.Message = $"驗證失敗: {callbackResult.StatusMessage}";
                    return result;
                }

                if (callbackResult.IsPaid)
                {
                    // TODO: 更新訂單狀態為已付款
                    // await UpdateOrderStatusAsync(callbackResult.OrderNo, OrderStatus.Paid);
                }

                result.Data = callbackResult.ResponseContent;
                result.Success();
            }
            catch (Exception ex)
            {
                result.Message = $"處理付款通知失敗: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 取號完成通知 (超商代碼/ATM)
        /// </summary>
        public virtual Result Customer()
        {
            var result = new Result();

            try
            {
                var provider = GetProvider();
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null)
                {
                    result.Message = "HttpContext 為空";
                    return result;
                }

                var callbackResult = provider.ValidateCallback(httpContext.Request.Form);

                if (!callbackResult.IsValid)
                {
                    result.Message = $"驗證失敗: {callbackResult.StatusMessage}";
                    return result;
                }

                // TODO: 更新訂單的繳費資訊
                // await UpdateOrderPaymentInfoAsync(callbackResult);

                result.Data = callbackResult.OrderNo;
                result.Success();
            }
            catch (Exception ex)
            {
                result.Message = $"處理取號通知失敗: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 轉換付款方式
        /// </summary>
        protected PaymentMethodType ConvertPaymentMethod(PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.Credit => PaymentMethodType.Credit,
                PaymentMethod.WebATM => PaymentMethodType.WebATM,
                PaymentMethod.ATM => PaymentMethodType.ATM,
                PaymentMethod.CVS => PaymentMethodType.CVS,
                PaymentMethod.BARCODE => PaymentMethodType.BARCODE,
                PaymentMethod.Union => PaymentMethodType.UnionPay,
                PaymentMethod.ALL => PaymentMethodType.ALL,
                _ => PaymentMethodType.ALL
            };
        }
    }
}

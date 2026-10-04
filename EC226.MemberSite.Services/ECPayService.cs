using EC226.Payment.Abstractions;
using EC226.Payment.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EC226.MemberSite.Services
{
    /// <summary>
    /// 綠界金流服務
    /// </summary>
    public class ECPayService : PaymentServiceBase
    {
        protected override PaymentProviderType ProviderType => PaymentProviderType.ECPay;

        public ECPayService(
            IConfiguration configuration,
            PaymentProviderFactory providerFactory,
            IHttpContextAccessor httpContextAccessor)
            : base(configuration, providerFactory, httpContextAccessor)
        {
        }

        protected override IPaymentProvider GetProvider()
        {
            return _providerFactory.CreateECPayProvider();
        }
    }
}

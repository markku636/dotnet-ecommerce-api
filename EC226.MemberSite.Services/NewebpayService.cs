using EC226.Payment.Abstractions;
using EC226.Payment.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EC226.MemberSite.Services
{
    /// <summary>
    /// 藍新金流服務
    /// </summary>
    public class NewebpayService : PaymentServiceBase
    {
        protected override PaymentProviderType ProviderType => PaymentProviderType.Newebpay;

        public NewebpayService(
            IConfiguration configuration,
            PaymentProviderFactory providerFactory,
            IHttpContextAccessor httpContextAccessor)
            : base(configuration, providerFactory, httpContextAccessor)
        {
        }

        protected override IPaymentProvider GetProvider()
        {
            return _providerFactory.CreateNewebpayProvider();
        }
    }
}

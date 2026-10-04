using System.Collections.Generic;
using Autofac;
using EC226.Services;
using EC226.Utility.Extensions;
using EC226.Pagination;
using EC226.Payment;
using EC226.Payment.Enums;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace EC226.MemberSite.Api.Controllers
{
    /// <summary>
    /// 金流控制器
    /// </summary>
    [Route("payment")]
    public class PaymentController : BaseController
    {
        private readonly IComponentContext _context;
        private readonly IPaymentService _paymentService;

        public PaymentController(IComponentContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        /// <summary>
        /// 取得付款選項列表
        /// </summary>
        [Route("paymentOption")]
        [HttpGet]
        public Result<List<PaymentVM>> GetPaymentOptions()
        {
            return _paymentService.GetPaymentOptions();
        }

        #region ECPay 綠界金流

        /// <summary>
        /// 綠界金流 - 付款完成通知 (Server POST)
        /// </summary>
        [Route("ecpay/notify")]
        [HttpPost]
        public IActionResult ECPayNotify()
        {
            var service = _context.ResolveNamed<IThirdPaymentService>(PaymentType.ECPAY.ToString());
            var result = service.NotifyPay();

            if (result)
            {
                return Content(result.Data);
            }

            return BadRequest();
        }

        /// <summary>
        /// 綠界物流 - 物流狀態通知
        /// </summary>
        [Route("ecpay/logistics/notify")]
        [HttpPost]
        [HttpGet]
        public IActionResult ECPayLogisticsNotify()
        {
            var service = _context.ResolveNamed<IThirdPaymentService>(PaymentType.ECPAY_Logistics.ToString());
            var result = service.NotifyPay();

            if (result)
            {
                return Content(result.Data);
            }

            return BadRequest();
        }

        #endregion

        #region Newebpay 藍新金流

        /// <summary>
        /// 藍新金流 - 付款完成通知 (Server POST)
        /// </summary>
        [Route("newebpay/notify")]
        [HttpPost]
        public IActionResult NewebpayNotify()
        {
            var service = _context.ResolveNamed<IThirdPaymentService>(PaymentType.Newebpay.ToString());
            var result = service.NotifyPay();

            if (result)
            {
                return Content(result.Data);
            }

            return BadRequest();
        }

        /// <summary>
        /// 藍新金流 - 取號完成通知 (超商代碼/ATM)
        /// </summary>
        [Route("newebpay/customer")]
        [HttpPost]
        public IActionResult NewebpayCustomer()
        {
            var service = _context.ResolveNamed<IThirdPaymentService>(PaymentType.Newebpay.ToString());
            var result = service.Customer();

            if (result)
            {
                return Redirect(PaymentAbstractService.GetHostUrl().UrlCombine($"page/order-success?orderNo={result.Data}"));
            }

            return Content(string.Empty);
        }

        #endregion

        #region 保留舊路由 (相容性)

        /// <summary>
        /// [已棄用] 請改用 /payment/ecpay/notify
        /// </summary>
        [Route("ecpayNotify")]
        [HttpPost]
        public IActionResult EcpayNotify_Legacy()
        {
            return ECPayNotify();
        }

        /// <summary>
        /// [已棄用] 請改用 /payment/ecpay/logistics/notify
        /// </summary>
        [Route("ecpayLogisticsNotify")]
        [HttpPost]
        [HttpGet]
        public IActionResult ECPayLogisticsNotify_Legacy()
        {
            return ECPayLogisticsNotify();
        }

        /// <summary>
        /// [已棄用] 請改用 /payment/newebpay/notify
        /// </summary>
        [Route("newebpayNotify")]
        [HttpPost]
        public IActionResult NewebpayNotify_Legacy()
        {
            return NewebpayNotify();
        }

        /// <summary>
        /// [已棄用] 請改用 /payment/newebpay/customer
        /// </summary>
        [Route("newebpayCustomer")]
        [HttpPost]
        public IActionResult NewebpayCustomer_Legacy(PaymentResult paymentResult, PaymentType paymentType = PaymentType.Newebpay)
        {
            return NewebpayCustomer();
        }

        #endregion
    }
}

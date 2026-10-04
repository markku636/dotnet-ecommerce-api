using System.Collections.Generic;
using Autofac;
using EC226.Pagination;
using EC226.Payment;
using EC226.Payment.Enums;
using EC226.MemberSite.IServices;
using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.Model.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;
namespace EC226.MemberSite.Api.Controllers
{
    [Route("testPayment")]
    public class TestPaymentController : BaseController
    {

        IComponentContext _context;
        public TestPaymentController(IComponentContext context)
        {
            this._context = context;
        }

        [Route("create")]
        [HttpGet]

        public Result<PaymentData> CreatePayForm(PaymentMethod paymentMethod = PaymentMethod.Credit, PaymentType paymentType = PaymentType.ECPAY_Logistics)
        {

            var service = _context.ResolveNamed<IThirdPaymentService>(paymentType.ToString());

            return service.TestCreatePayForm(paymentMethod);
        }

        [Route("ecpayNotify")]
        [HttpPost]
        public IActionResult EcpayNotify()
        {
            var p = _context.ResolveNamed<IThirdPaymentService>(PaymentType.Newebpay.ToString());

            var result = p.NotifyPay();

            if (result)
            {
                return Content(result.Data);
            }
            else
            {
                return BadRequest();
            }

        }

        [Route("ecpayLogisticsNotify")]
        [HttpPost]
        [HttpGet]
        public IActionResult ECPayLogisticsNotify()
        {
            var p = _context.ResolveNamed<IThirdPaymentService>(PaymentType.ECPAY_Logistics.ToString());

            var result = p.NotifyPay();

            if (result)
            {
                return Content(result.Data);
            }
            else
            {
                return BadRequest();
            }

        }       



        [Route("newebpayNotify")]
        [HttpPost]
        public IActionResult NewebpayNotify()
        {

            var p = _context.ResolveNamed<IThirdPaymentService>(PaymentType.Newebpay.ToString());

            var result = p.NotifyPay();

            if (result)
            {
                return Content(result.Data);
            }
            else
            {
                return BadRequest();
            }

        }

        // 藍新超取
        [Route("customer")]
        [HttpPost]
        public IActionResult Customer(PaymentResult paymentResult, PaymentType paymentType = PaymentType.Newebpay)
        {

            var p = _context.ResolveNamed<IThirdPaymentService>(paymentType.ToString());

            var result = p.Customer();

            if (result)
            {
                return Ok(result.Data);
            }

            return Ok(string.Empty);

        }
    }
}

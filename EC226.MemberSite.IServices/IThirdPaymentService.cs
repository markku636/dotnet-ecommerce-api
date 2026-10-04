using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.IServices;
using EC226.Payment;
using System;
using System.Collections.Generic;
using System.Text;
using EC226.MemberSite.Model.ViewModels.UI;

namespace EC226.MemberSite.IServices
{
    public interface IThirdPaymentService
    {

        // 建立測試付款表單
        public Result<PaymentData> TestCreatePayForm(PaymentMethod paymentMethod);

        // 建立付款表單
        public Result<PaymentData> CreatePayForm(PaymentParameter paymentParameter);

        // 回調通知
        public Result NotifyPay();

        // 藍新專用 for 取號完成系統通知 ( 超商代碼繳費交易流程)
        public Result Customer();
    }
}

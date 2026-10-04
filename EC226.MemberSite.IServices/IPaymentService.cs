using EC226.MemberSite.Model.ViewModels;
using EC226.MemberSite.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using EC226.MemberSite.Model.ViewModels.UI;

namespace EC226.MemberSite.IServices
{
    public interface IPaymentService
    {
        public Result<List<PaymentVM>> GetPaymentOptions();
    }
}

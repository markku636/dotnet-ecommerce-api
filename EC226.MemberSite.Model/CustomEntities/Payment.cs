using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{

    public partial class Payment
    {

      // 與一對一及多對一相反 第一個 從表字段，第二個主表字段
      [Navigate(NavigateType.OneToMany, nameof(PaymentOption.PaymentId), nameof(Id))]
      public List<PaymentOption> PaymentOptions { get; set; }

   }
}

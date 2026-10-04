using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{

    public partial class Order
    {

      // 與一對一及多對一相反 第一個 從表字段，第二個主表字段
      [Navigate(NavigateType.OneToMany, nameof(OrderDetail.OrderId), nameof(Id))]
      public List<OrderDetail> OrderDetails { get; set; }

   }
}

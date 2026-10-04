using EC226.MemberSite.Model.Entities;
using EC226.MemberSite.Model.ViewModels;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
   public partial class StandardProductUnit
   {

      // 與一對一及多對一相反 第一個 從表字段，第二個主表字段
      [Navigate(NavigateType.OneToMany, nameof(StandardProductUnitImage.StandardProductUnitId), nameof(Id))]
      public List<StandardProductUnitImage> StandardProductUnitImages { get; set; }

      // 與一對一及多對一相反 第一個 從表字段，第二個主表字段
      [Navigate(NavigateType.OneToMany, nameof(StockKeepingUnit.StandardProductUnitId), nameof(Id))]
      public List<StockKeepingUnit> StockKeepingUnits { get; set; }          

      // 第一個主表字段，第二從表字段  順序不要錯了
      [Navigate(NavigateType.OneToOne, nameof(CategoryId), nameof(Category.Id))]
      public Category Category { get; set; }       

      // 與一對一及多對一相反 第一個 從表字段，第二個主表字段
      [Navigate(NavigateType.OneToMany, nameof(StandardProductUnitCarousel.StandardProductUnitId), nameof(Id))]
      public List<StandardProductUnitCarousel> StandardProductUnitCarousels { get; set; }


      // 與一對一及多對一相反 第一個 從表字段，第二個主表字段
      [Navigate(NavigateType.OneToMany, nameof(OrderDetail.StandardProductUnitId), nameof(Id))]
      public List<OrderDetail> OrderDetails { get; set; }

      // 與一對一及多對一相反 第一個 從表字段，第二個主表字段
      [Navigate(NavigateType.OneToMany, nameof(StockKeepingUnit.StandardProductUnitId), nameof(Id))]
      public List<StockKeepingUnitStock> StockKeepingUnitStocks { get; set; }

   }
}

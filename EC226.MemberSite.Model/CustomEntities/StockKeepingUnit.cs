using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    
    public partial class StockKeepingUnit
   {     

      // 與一對一及多對一相反 第一個 從表字段，第二個主表字段
      [Navigate(NavigateType.OneToMany, nameof(SpecificationOptionRelation.StockKeepingUnitId), nameof(Id))]
      public List<SpecificationOptionRelation> SpecificationOptionRelations { get; set; }


      // 與一對一及多對一相反 第一個 從表字段，第二個主表字段
      [Navigate(NavigateType.OneToMany, nameof(StandardProductUnitCarousel.StockKeepingUnitId), nameof(Id))]
      public List<StandardProductUnitCarousel> StandardProductUnitCarousels { get; set; }

   }
}

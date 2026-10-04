using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    
    public partial class Specification
    {     

      // 與一對一及多對一相反 第一個 從表字段，第二個主表字段
      [Navigate(NavigateType.OneToMany, nameof(SpecificationOption.SpecificationId), nameof(Id))]
      public List<SpecificationOptionRelation> SpecificationOptionRelations { get; set; }


      // 與一對一及多對一相反 第一個 從表字段，第二個主表字段
      [Navigate(NavigateType.OneToMany, nameof(SpecificationOption.SpecificationId), nameof(Id))]
      public List<SpecificationOption> SpecificationOptions { get; set; }

   }
}

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// SpecificationOptionRelation
    /// </summary>
    
    public partial class SpecificationOptionRelation
   {     
      [Navigate(NavigateType.ManyToOne, nameof(SpecificationOptionId), nameof(SpecificationOption.Id))]
      public SpecificationOption SpecificationOption { get; set; }

      [Navigate(NavigateType.ManyToOne, nameof(SpecificationId), nameof(Specification.Id))]
      public Specification Specification { get; set; }
   }
}

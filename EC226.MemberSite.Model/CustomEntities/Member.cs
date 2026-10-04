using EC226.MemberSite.Model.ViewModels;
using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
 
    public partial class Member
    {
      [Navigate(NavigateType.OneToMany, nameof(RecipienAddress.MemberId), nameof(Id))]
      public List<RecipienAddress> RecipienAddresses { get; set; }
   }
}

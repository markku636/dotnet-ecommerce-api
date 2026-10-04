using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// MemberRolesRelations
    /// </summary>
    [SugarTable("MemberRolesRelations")]
    public partial class MemberRolesRelations
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public MemberRolesRelations()
        {
        }
		        /// <summary>
        /// RoleId
        /// </summary>
        [Display(Name = "RoleId")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 RoleId  { get; set; }
        
		        /// <summary>
        /// MemberId
        /// </summary>
        [Display(Name = "MemberId")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 MemberId  { get; set; }
        
		
    }
}

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// AccountRolesRelations
    /// </summary>
    [SugarTable("AccountRolesRelations")]
    public partial class AccountRolesRelations
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public AccountRolesRelations()
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
        /// AccountId
        /// </summary>
        [Display(Name = "AccountId")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 AccountId  { get; set; }
        
		
    }
}

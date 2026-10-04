using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// ProgramRolesRelations
    /// </summary>
    [SugarTable("ProgramRolesRelations")]
    public partial class ProgramRolesRelations
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public ProgramRolesRelations()
        {
        }
		        /// <summary>
        /// Roles_Id
        /// </summary>
        [Display(Name = "Roles_Id")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Roles_Id  { get; set; }
        
		        /// <summary>
        /// Programs_Id
        /// </summary>
        [Display(Name = "Programs_Id")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Programs_Id  { get; set; }
        
		
    }
}

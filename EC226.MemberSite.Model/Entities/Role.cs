using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// Role
    /// </summary>
    [SugarTable("Role")]
    public partial class Role
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public Role()
        {
        }
		        /// <summary>
        /// Id
        /// </summary>
        [Display(Name = "Id")]
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Id  { get; set; }
        
		        /// <summary>
        /// Name
        /// </summary>
        [Display(Name = "Name")]
        [Required(ErrorMessage = "請輸入{0}")]
		  public System.String Name  { get; set; }
        
		        /// <summary>
        /// SpecialToken
        /// </summary>
        [Display(Name = "SpecialToken")]
        
		  public System.String SpecialToken  { get; set; }
        
		        /// <summary>
        /// CreateTime
        /// </summary>
        [Display(Name = "CreateTime")]
        
        public System.DateTime? CreateTime { get; set; }
        
		        /// <summary>
        /// ModifyTime
        /// </summary>
        [Display(Name = "ModifyTime")]
        
        public System.DateTime? ModifyTime { get; set; }
        
		        /// <summary>
        /// Status
        /// </summary>
        [Display(Name = "Status")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Status  { get; set; }
        
		        /// <summary>
        /// IsDelete
        /// </summary>
        [Display(Name = "IsDelete")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean IsDelete  { get; set; }
        
		        /// <summary>
        /// Sort
        /// </summary>
        [Display(Name = "Sort")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Sort  { get; set; }
        
		
    }
}

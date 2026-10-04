using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// Account
    /// </summary>
    [SugarTable("Account")]
    public partial class Account
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public Account()
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
        /// UserAccount
        /// </summary>
        [Display(Name = "UserAccount")]
        [Required(ErrorMessage = "請輸入{0}")]
		  public System.String UserAccount  { get; set; }
        
		        /// <summary>
        /// Password
        /// </summary>
        [Display(Name = "Password")]
        
		  public System.String Password  { get; set; }
        
		        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Email")]
        
		  public System.String Email  { get; set; }
        
		        /// <summary>
        /// UserName
        /// </summary>
        [Display(Name = "UserName")]
        
		  public System.String UserName  { get; set; }
        
		        /// <summary>
        /// CreateTime
        /// </summary>
        [Display(Name = "CreateTime")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.DateTime CreateTime { get; set; }
        
		        /// <summary>
        /// ModifyTime
        /// </summary>
        [Display(Name = "ModifyTime")]
        
        public System.DateTime? ModifyTime { get; set; }
        
		        /// <summary>
        /// Status
        /// </summary>
        [Display(Name = "Status")]
        
        public System.Int32? Status  { get; set; }
        
		        /// <summary>
        /// Sort
        /// </summary>
        [Display(Name = "Sort")]
        
        public System.Int32? Sort  { get; set; }
        
		        /// <summary>
        /// IsDelete
        /// </summary>
        [Display(Name = "IsDelete")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean IsDelete  { get; set; }
        
		
    }
}

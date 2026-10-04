using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// Member
    /// </summary>
    [SugarTable("Member")]
    public partial class Member
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public Member()
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
        /// DisplayName
        /// </summary>
        [Display(Name = "DisplayName")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:90,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String DisplayName  { get; set; }
        
		        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Email")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:90,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Email  { get; set; }
        
		        /// <summary>
        /// Password
        /// </summary>
        [Display(Name = "Password")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:256,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Password  { get; set; }
        
		        /// <summary>
        /// Hash
        /// </summary>
        [Display(Name = "Hash")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Hash  { get; set; }
        
		        /// <summary>
        /// CreateTime
        /// </summary>
        [Display(Name = "CreateTime")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.DateTime CreateTime { get; set; }
        
		        /// <summary>
        /// Enable
        /// </summary>
        [Display(Name = "Enable")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean Enable  { get; set; }
        
		        /// <summary>
        /// Phone
        /// </summary>
        [Display(Name = "Phone")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Phone  { get; set; }
        
		        /// <summary>
        /// Token
        /// </summary>
        [Display(Name = "Token")]
        [StringLength(maximumLength:300,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Token  { get; set; }
        
		        /// <summary>
        /// ModifyTime
        /// </summary>
        [Display(Name = "ModifyTime")]
        
        public System.DateTime? ModifyTime { get; set; }
        
		        /// <summary>
        /// HasChangePassword
        /// </summary>
        [Display(Name = "HasChangePassword")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean HasChangePassword  { get; set; }
        
		
    }
}

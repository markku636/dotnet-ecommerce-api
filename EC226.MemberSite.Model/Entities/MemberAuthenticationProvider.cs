using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// MemberAuthenticationProvider
    /// </summary>
    [SugarTable("MemberAuthenticationProvider")]
    public partial class MemberAuthenticationProvider
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public MemberAuthenticationProvider()
        {
        }
		        /// <summary>
        /// ProviderMemberId
        /// </summary>
        [Display(Name = "ProviderMemberId")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String ProviderMemberId  { get; set; }
        
		        /// <summary>
        /// FirebaseMemberId
        /// </summary>
        [Display(Name = "FirebaseMemberId")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String FirebaseMemberId  { get; set; }
        
		        /// <summary>
        /// ProviderId
        /// </summary>
        [Display(Name = "ProviderId")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String ProviderId  { get; set; }
        
		        /// <summary>
        /// MemberId
        /// </summary>
        [Display(Name = "MemberId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 MemberId  { get; set; }
        
		        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Email")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:128,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Email  { get; set; }
        
		        /// <summary>
        /// DisplayName
        /// </summary>
        [Display(Name = "DisplayName")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String DisplayName  { get; set; }
        
		        /// <summary>
        /// CreateDateTime
        /// </summary>
        [Display(Name = "CreateDateTime")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.DateTime CreateDateTime { get; set; }
        
		        /// <summary>
        /// Enable
        /// </summary>
        [Display(Name = "Enable")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean Enable  { get; set; }
        
		
    }
}

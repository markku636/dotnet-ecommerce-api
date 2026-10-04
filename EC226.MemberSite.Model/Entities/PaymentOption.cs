using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// PaymentOption
    /// </summary>
    [SugarTable("PaymentOption")]
    public partial class PaymentOption
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public PaymentOption()
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
        /// PaymentId
        /// </summary>
        [Display(Name = "PaymentId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 PaymentId  { get; set; }
        
		        /// <summary>
        /// Code
        /// </summary>
        [Display(Name = "Code")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:20,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Code  { get; set; }
        
		        /// <summary>
        /// SubCode
        /// </summary>
        [Display(Name = "SubCode")]
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String SubCode  { get; set; }
        
		        /// <summary>
        /// Description
        /// </summary>
        [Display(Name = "Description")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Description  { get; set; }
        
		        /// <summary>
        /// Sort
        /// </summary>
        [Display(Name = "Sort")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Sort  { get; set; }
        
		        /// <summary>
        /// Enable
        /// </summary>
        [Display(Name = "Enable")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean Enable  { get; set; }
        
		        /// <summary>
        /// DisplayName
        /// </summary>
        [Display(Name = "DisplayName")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String DisplayName  { get; set; }
        
		
    }
}

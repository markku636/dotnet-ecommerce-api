using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// Payment
    /// </summary>
    [SugarTable("Payment")]
    public partial class Payment
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public Payment()
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
        /// Code
        /// </summary>
        [Display(Name = "Code")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Code  { get; set; }
        
		        /// <summary>
        /// Name
        /// </summary>
        [Display(Name = "Name")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Name  { get; set; }
        
		        /// <summary>
        /// DisplayName
        /// </summary>
        [Display(Name = "DisplayName")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String DisplayName  { get; set; }
        
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
        
		
    }
}

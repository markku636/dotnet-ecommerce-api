using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// RecipienAddress
    /// </summary>
    [SugarTable("RecipienAddress")]
    public partial class RecipienAddress
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public RecipienAddress()
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
        /// MemberId
        /// </summary>
        [Display(Name = "MemberId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 MemberId  { get; set; }
        
		        /// <summary>
        /// RecipientName
        /// </summary>
        [Display(Name = "RecipientName")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String RecipientName  { get; set; }
        
		        /// <summary>
        /// Phone
        /// </summary>
        [Display(Name = "Phone")]
        [Required(ErrorMessage = "請輸入{0}")]
		  public System.String Phone  { get; set; }
        
		        /// <summary>
        /// Country
        /// </summary>
        [Display(Name = "Country")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:3,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Country  { get; set; }
        
		        /// <summary>
        /// ZipCode
        /// </summary>
        [Display(Name = "ZipCode")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:3,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String ZipCode  { get; set; }
        
		        /// <summary>
        /// Address
        /// </summary>
        [Display(Name = "Address")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Address  { get; set; }
        
		        /// <summary>
        /// IsDefault
        /// </summary>
        [Display(Name = "IsDefault")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean IsDefault  { get; set; }
        
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
        
		
    }
}

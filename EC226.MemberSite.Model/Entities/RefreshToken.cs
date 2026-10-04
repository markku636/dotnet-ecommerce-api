using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// RefreshToken
    /// </summary>
    [SugarTable("RefreshToken")]
    public partial class RefreshToken
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public RefreshToken()
        {
        }
		        /// <summary>
        /// Id
        /// </summary>
        [Display(Name = "Id")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Id  { get; set; }
        
		        /// <summary>
        /// MemberId
        /// </summary>
        [Display(Name = "MemberId")]
        
        public System.Int32? MemberId  { get; set; }
        
		        /// <summary>
        /// Token
        /// </summary>
        [Display(Name = "Token")]
        [StringLength(maximumLength:150,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Token  { get; set; }
        
		        /// <summary>
        /// JwtId
        /// </summary>
        [Display(Name = "JwtId")]
        [StringLength(maximumLength:150,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String JwtId  { get; set; }
        
		        /// <summary>
        /// IsUsed
        /// </summary>
        [Display(Name = "IsUsed")]
        
        public System.Boolean? IsUsed  { get; set; }
        
		        /// <summary>
        /// IsRevorked
        /// </summary>
        [Display(Name = "IsRevorked")]
        
        public System.Boolean? IsRevorked  { get; set; }
        
		        /// <summary>
        /// AddedDate
        /// </summary>
        [Display(Name = "AddedDate")]
        
        public System.DateTime? AddedDate { get; set; }
        
		        /// <summary>
        /// ExpiryDate
        /// </summary>
        [Display(Name = "ExpiryDate")]
        
        public System.DateTime? ExpiryDate { get; set; }
        
		        /// <summary>
        /// UserName
        /// </summary>
        [Display(Name = "UserName")]
        [StringLength(maximumLength:150,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String UserName  { get; set; }
        
		
    }
}

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// Article
    /// </summary>
    [SugarTable("Article")]
    public partial class Article
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public Article()
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
        /// Title
        /// </summary>
        [Display(Name = "Title")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:150,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Title  { get; set; }
        
		        /// <summary>
        /// Content
        /// </summary>
        [Display(Name = "Content")]
        
		  public System.String Content  { get; set; }
        
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
        /// IsDelete
        /// </summary>
        [Display(Name = "IsDelete")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean IsDelete  { get; set; }
        
		        /// <summary>
        /// CoverUrl
        /// </summary>
        [Display(Name = "CoverUrl")]
        [StringLength(maximumLength:150,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String CoverUrl  { get; set; }
        
		        /// <summary>
        /// Author
        /// </summary>
        [Display(Name = "Author")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:30,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Author  { get; set; }
        
		        /// <summary>
        /// Enable
        /// </summary>
        [Display(Name = "Enable")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean Enable  { get; set; }
        
		        /// <summary>
        /// Hit
        /// </summary>
        [Display(Name = "Hit")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Hit  { get; set; }
        
		        /// <summary>
        /// IsHot
        /// </summary>
        [Display(Name = "IsHot")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean IsHot  { get; set; }
        
		
    }
}

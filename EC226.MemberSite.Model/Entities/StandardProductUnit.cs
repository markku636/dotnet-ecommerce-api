using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// StandardProductUnit
    /// </summary>
    [SugarTable("StandardProductUnit")]
    public partial class StandardProductUnit
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public StandardProductUnit()
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
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Name  { get; set; }
        
		        /// <summary>
        /// Ingredients
        /// </summary>
        [Display(Name = "Ingredients")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Ingredients  { get; set; }
        
		        /// <summary>
        /// Description
        /// </summary>
        [Display(Name = "Description")]
        
		  public System.String Description  { get; set; }
        
		        /// <summary>
        /// CategoryId
        /// </summary>
        [Display(Name = "CategoryId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 CategoryId  { get; set; }
        
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
        /// CoverUrl
        /// </summary>
        [Display(Name = "CoverUrl")]
        [StringLength(maximumLength:150,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String CoverUrl  { get; set; }
        
		        /// <summary>
        /// IsHot
        /// </summary>
        [Display(Name = "IsHot")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean IsHot  { get; set; }
        
		
    }
}

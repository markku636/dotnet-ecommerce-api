using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// StandardProductUnitCarousel
    /// </summary>
    [SugarTable("StandardProductUnitCarousel")]
    public partial class StandardProductUnitCarousel
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public StandardProductUnitCarousel()
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
        /// LargeImage
        /// </summary>
        [Display(Name = "LargeImage")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:255,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String LargeImage  { get; set; }
        
		        /// <summary>
        /// MidImage
        /// </summary>
        [Display(Name = "MidImage")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:255,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String MidImage  { get; set; }
        
		        /// <summary>
        /// SmaillImage
        /// </summary>
        [Display(Name = "SmaillImage")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:255,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String SmaillImage  { get; set; }
        
		        /// <summary>
        /// StandardProductUnitId
        /// </summary>
        [Display(Name = "StandardProductUnitId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 StandardProductUnitId  { get; set; }
        
		        /// <summary>
        /// StockKeepingUnitId
        /// </summary>
        [Display(Name = "StockKeepingUnitId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 StockKeepingUnitId  { get; set; }
        
		
    }
}

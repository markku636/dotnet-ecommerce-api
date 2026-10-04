using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// StockKeepingUnitCarousel
    /// </summary>
    [SugarTable("StockKeepingUnitCarousel")]
    public partial class StockKeepingUnitCarousel
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public StockKeepingUnitCarousel()
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
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:150,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String LargeImage  { get; set; }
        
		        /// <summary>
        /// MidImage
        /// </summary>
        [Display(Name = "MidImage")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:150,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String MidImage  { get; set; }
        
		        /// <summary>
        /// SmaillImage
        /// </summary>
        [Display(Name = "SmaillImage")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:150,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String SmaillImage  { get; set; }
        
		        /// <summary>
        /// Sort
        /// </summary>
        [Display(Name = "Sort")]
        
        public System.Int32? Sort  { get; set; }
        
		
    }
}

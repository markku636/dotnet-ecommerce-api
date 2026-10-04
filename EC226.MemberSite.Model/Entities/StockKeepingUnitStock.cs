using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// StockKeepingUnitStock
    /// </summary>
    [SugarTable("StockKeepingUnitStock")]
    public partial class StockKeepingUnitStock
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public StockKeepingUnitStock()
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
        /// StandardProductUnitId
        /// </summary>
        [Display(Name = "StandardProductUnitId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 StandardProductUnitId  { get; set; }
        
		        /// <summary>
        /// Price
        /// </summary>
        [Display(Name = "Price")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Price  { get; set; }
        
		        /// <summary>
        /// Stock
        /// </summary>
        [Display(Name = "Stock")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Stock  { get; set; }
        
		        /// <summary>
        /// Cost
        /// </summary>
        [Display(Name = "Cost")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Cost  { get; set; }
        
		        /// <summary>
        /// SKUs
        /// </summary>
        [Display(Name = "SKUs")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String SKUs  { get; set; }
        
		        /// <summary>
        /// CreateDate
        /// </summary>
        [Display(Name = "CreateDate")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.DateTime CreateDate { get; set; }
        
		        /// <summary>
        /// Memo
        /// </summary>
        [Display(Name = "Memo")]
        [StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Memo  { get; set; }
        
		
    }
}

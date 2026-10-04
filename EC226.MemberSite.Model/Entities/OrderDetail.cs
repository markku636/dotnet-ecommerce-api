using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// OrderDetail
    /// </summary>
    [SugarTable("OrderDetail")]
    public partial class OrderDetail
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public OrderDetail()
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
        /// OrderId
        /// </summary>
        [Display(Name = "OrderId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 OrderId  { get; set; }
        
		        /// <summary>
        /// StandardProductUnitId
        /// </summary>
        [Display(Name = "StandardProductUnitId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 StandardProductUnitId  { get; set; }
        
		        /// <summary>
        /// SKUs
        /// </summary>
        [Display(Name = "SKUs")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String SKUs  { get; set; }
        
		        /// <summary>
        /// Quantity
        /// </summary>
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Quantity  { get; set; }
        
		        /// <summary>
        /// Price
        /// </summary>
        [Display(Name = "Price")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Price  { get; set; }
        
		        /// <summary>
        /// SubTotal
        /// </summary>
        [Display(Name = "SubTotal")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 SubTotal  { get; set; }
        
		        /// <summary>
        /// Memo
        /// </summary>
        [Display(Name = "Memo")]
        [StringLength(maximumLength:100,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Memo  { get; set; }
        
		        /// <summary>
        /// SpuName
        /// </summary>
        [Display(Name = "SpuName")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String SpuName  { get; set; }
        
		        /// <summary>
        /// SpecOptionName
        /// </summary>
        [Display(Name = "SpecOptionName")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String SpecOptionName  { get; set; }
        
		
    }
}

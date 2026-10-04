using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// StockKeepingUnit
    /// </summary>
    [SugarTable("StockKeepingUnit")]
    public partial class StockKeepingUnit
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public StockKeepingUnit()
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
        /// Name
        /// </summary>
        [Display(Name = "Name")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Name  { get; set; }
        
		        /// <summary>
        /// MainPicture
        /// </summary>
        [Display(Name = "MainPicture")]
        [StringLength(maximumLength:150,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String MainPicture  { get; set; }
        
		        /// <summary>
        /// ShowImage
        /// </summary>
        [Display(Name = "ShowImage")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean ShowImage  { get; set; }
        
		
    }
}

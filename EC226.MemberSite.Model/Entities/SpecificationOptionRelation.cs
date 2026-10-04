using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// SpecificationOptionRelation
    /// </summary>
    [SugarTable("SpecificationOptionRelation")]
    public partial class SpecificationOptionRelation
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public SpecificationOptionRelation()
        {
        }
		        /// <summary>
        /// StandardProductUnitId
        /// </summary>
        [Display(Name = "StandardProductUnitId")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 StandardProductUnitId  { get; set; }
        
		        /// <summary>
        /// StockKeepingUnitId
        /// </summary>
        [Display(Name = "StockKeepingUnitId")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 StockKeepingUnitId  { get; set; }
        
		        /// <summary>
        /// SpecificationId
        /// </summary>
        [Display(Name = "SpecificationId")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 SpecificationId  { get; set; }
        
		        /// <summary>
        /// SpecificationOptionId
        /// </summary>
        [Display(Name = "SpecificationOptionId")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 SpecificationOptionId  { get; set; }
        
		        /// <summary>
        /// CreateDate
        /// </summary>
        [Display(Name = "CreateDate")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.DateTime CreateDate { get; set; }
        
		
    }
}

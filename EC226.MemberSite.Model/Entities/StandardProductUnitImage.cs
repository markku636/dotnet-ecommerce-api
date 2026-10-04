using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// StandardProductUnitImage
    /// </summary>
    [SugarTable("StandardProductUnitImage")]
    public partial class StandardProductUnitImage
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public StandardProductUnitImage()
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
        /// ImagePath
        /// </summary>
        [Display(Name = "ImagePath")]
        [StringLength(maximumLength:150,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String ImagePath  { get; set; }
        
		
    }
}

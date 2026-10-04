using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// SpecificationOption
    /// </summary>
    [SugarTable("SpecificationOption")]
    public partial class SpecificationOption
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public SpecificationOption()
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
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Name  { get; set; }
        
		        /// <summary>
        /// SpecificationId
        /// </summary>
        [Display(Name = "SpecificationId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 SpecificationId  { get; set; }
        
		
    }
}

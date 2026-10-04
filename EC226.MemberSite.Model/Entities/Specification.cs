using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// Specification
    /// </summary>
    [SugarTable("Specification")]
    public partial class Specification
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public Specification()
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
        /// Sort
        /// </summary>
        [Display(Name = "Sort")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Sort  { get; set; }
        
		        /// <summary>
        /// CategoryId
        /// </summary>
        [Display(Name = "CategoryId")]
        
        public System.Int32? CategoryId  { get; set; }
        
		
    }
}

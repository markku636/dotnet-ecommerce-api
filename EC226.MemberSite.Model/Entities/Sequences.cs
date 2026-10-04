using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// Sequences
    /// </summary>
    [SugarTable("Sequences")]
    public partial class Sequences
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public Sequences()
        {
        }
		        /// <summary>
        /// SequenceDate
        /// </summary>
        [Display(Name = "SequenceDate")]
        [SugarColumn(IsPrimaryKey = true)]
[Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String SequenceDate  { get; set; }
        
		        /// <summary>
        /// Number
        /// </summary>
        [Display(Name = "Number")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Number  { get; set; }
        
		        /// <summary>
        /// SequenceType
        /// </summary>
        [Display(Name = "SequenceType")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 SequenceType  { get; set; }
        
		
    }
}

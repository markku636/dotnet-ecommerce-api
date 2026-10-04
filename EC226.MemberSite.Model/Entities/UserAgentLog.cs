using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// UserAgentLog
    /// </summary>
    [SugarTable("UserAgentLog")]
    public partial class UserAgentLog
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public UserAgentLog()
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
        /// UserAgent
        /// </summary>
        [Display(Name = "UserAgent")]
        
		  public System.String UserAgent  { get; set; }
        
		        /// <summary>
        /// IP
        /// </summary>
        [Display(Name = "IP")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String IP  { get; set; }
        
		        /// <summary>
        /// Header
        /// </summary>
        [Display(Name = "Header")]
        
		  public System.String Header  { get; set; }
        
		        /// <summary>
        /// Path
        /// </summary>
        [Display(Name = "Path")]
        [StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Path  { get; set; }
        
		        /// <summary>
        /// DateTime
        /// </summary>
        [Display(Name = "DateTime")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.DateTime DateTime { get; set; }
        
		
    }
}

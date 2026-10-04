using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// Program
    /// </summary>
    [SugarTable("Program")]
    public partial class Program
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public Program()
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
        
		  public System.String Name  { get; set; }
        
		        /// <summary>
        /// Module
        /// </summary>
        [Display(Name = "Module")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Module  { get; set; }
        
		        /// <summary>
        /// HttpMethod
        /// </summary>
        [Display(Name = "HttpMethod")]
        [StringLength(maximumLength:30,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String HttpMethod  { get; set; }
        
		        /// <summary>
        /// ControllerFullName
        /// </summary>
        [Display(Name = "ControllerFullName")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String ControllerFullName  { get; set; }
        
		        /// <summary>
        /// IdentityKey
        /// </summary>
        [Display(Name = "IdentityKey")]
        [StringLength(maximumLength:300,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String IdentityKey  { get; set; }
        
		        /// <summary>
        /// Controller
        /// </summary>
        [Display(Name = "Controller")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:100,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Controller  { get; set; }
        
		        /// <summary>
        /// Action
        /// </summary>
        [Display(Name = "Action")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:100,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Action  { get; set; }
        
		        /// <summary>
        /// ReturnType
        /// </summary>
        [Display(Name = "ReturnType")]
        [StringLength(maximumLength:100,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String ReturnType  { get; set; }
        
		        /// <summary>
        /// ParameterCount
        /// </summary>
        [Display(Name = "ParameterCount")]
        
        public System.Int32? ParameterCount  { get; set; }
        
		        /// <summary>
        /// Describe
        /// </summary>
        [Display(Name = "Describe")]
        [StringLength(maximumLength:300,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Describe  { get; set; }
        
		        /// <summary>
        /// CreateTime
        /// </summary>
        [Display(Name = "CreateTime")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.DateTime CreateTime { get; set; }
        
		        /// <summary>
        /// SiteType
        /// </summary>
        [Display(Name = "SiteType")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 SiteType  { get; set; }
        
		        /// <summary>
        /// Sort
        /// </summary>
        [Display(Name = "Sort")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Sort  { get; set; }
        
		
    }
}

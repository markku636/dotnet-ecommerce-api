using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// Transaction
    /// </summary>
    [SugarTable("Transaction")]
    public partial class Transaction
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public Transaction()
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
        /// OrderNo
        /// </summary>
        [Display(Name = "OrderNo")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:100,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String OrderNo  { get; set; }
        
		        /// <summary>
        /// TradeNo
        /// </summary>
        [Display(Name = "TradeNo")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:100,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String TradeNo  { get; set; }
        
		        /// <summary>
        /// PayTime
        /// </summary>
        [Display(Name = "PayTime")]
        
        public System.DateTime? PayTime { get; set; }
        
		        /// <summary>
        /// IP
        /// </summary>
        [Display(Name = "IP")]
        [StringLength(maximumLength:128,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String IP  { get; set; }
        
		        /// <summary>
        /// Message
        /// </summary>
        [Display(Name = "Message")]
        [StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Message  { get; set; }
        
		        /// <summary>
        /// Amount
        /// </summary>
        [Display(Name = "Amount")]
        
        public System.Int32? Amount  { get; set; }
        
		        /// <summary>
        /// Card6No
        /// </summary>
        [Display(Name = "Card6No")]
        [StringLength(maximumLength:6,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Card6No  { get; set; }
        
		        /// <summary>
        /// Card4No
        /// </summary>
        [Display(Name = "Card4No")]
        [StringLength(maximumLength:4,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Card4No  { get; set; }
        
		        /// <summary>
        /// NotifyIp
        /// </summary>
        [Display(Name = "NotifyIp")]
        [StringLength(maximumLength:128,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String NotifyIp  { get; set; }
        
		        /// <summary>
        /// StoreName
        /// </summary>
        [Display(Name = "StoreName")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String StoreName  { get; set; }
        
		        /// <summary>
        /// StoreType
        /// </summary>
        [Display(Name = "StoreType")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String StoreType  { get; set; }
        
		        /// <summary>
        /// StoreAddress
        /// </summary>
        [Display(Name = "StoreAddress")]
        [StringLength(maximumLength:128,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String StoreAddress  { get; set; }
        
		        /// <summary>
        /// ReceviceName
        /// </summary>
        [Display(Name = "ReceviceName")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String ReceviceName  { get; set; }
        
		        /// <summary>
        /// RecipientPhone
        /// </summary>
        [Display(Name = "RecipientPhone")]
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String RecipientPhone  { get; set; }
        
		        /// <summary>
        /// TradeType
        /// </summary>
        [Display(Name = "TradeType")]
        
        public System.Int32? TradeType  { get; set; }
        
		
    }
}

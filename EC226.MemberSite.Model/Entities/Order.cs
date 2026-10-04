using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC226.MemberSite.Model.Entities
{
    /// <summary>
    /// Order
    /// </summary>
    [SugarTable("Order")]
    public partial class Order
    {
        /// <summary>
        /// 建構函式
        /// </summary>
        public Order()
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
        /// RecipientName
        /// </summary>
        [Display(Name = "RecipientName")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String RecipientName  { get; set; }
        
		        /// <summary>
        /// Phone
        /// </summary>
        [Display(Name = "Phone")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Phone  { get; set; }
        
		        /// <summary>
        /// Country
        /// </summary>
        [Display(Name = "Country")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:50,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Country  { get; set; }
        
		        /// <summary>
        /// ZipCode
        /// </summary>
        [Display(Name = "ZipCode")]
        [StringLength(maximumLength:3,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String ZipCode  { get; set; }
        
		        /// <summary>
        /// Address
        /// </summary>
        [Display(Name = "Address")]
        [StringLength(maximumLength:200,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String Address  { get; set; }
        
		        /// <summary>
        /// Date
        /// </summary>
        [Display(Name = "Date")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.DateTime Date { get; set; }
        
		        /// <summary>
        /// OrderNo
        /// </summary>
        [Display(Name = "OrderNo")]
        [Required(ErrorMessage = "請輸入{0}")][StringLength(maximumLength:100,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String OrderNo  { get; set; }
        
		        /// <summary>
        /// Status
        /// </summary>
        [Display(Name = "Status")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 Status  { get; set; }
        
		        /// <summary>
        /// FinalAmount
        /// </summary>
        [Display(Name = "FinalAmount")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 FinalAmount  { get; set; }
        
		        /// <summary>
        /// MemberId
        /// </summary>
        [Display(Name = "MemberId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 MemberId  { get; set; }
        
		        /// <summary>
        /// PaymentId
        /// </summary>
        [Display(Name = "PaymentId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 PaymentId  { get; set; }
        
		        /// <summary>
        /// PaymentOptionId
        /// </summary>
        [Display(Name = "PaymentOptionId")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 PaymentOptionId  { get; set; }
        
		        /// <summary>
        /// EstimatedDeliveryDate
        /// </summary>
        [Display(Name = "EstimatedDeliveryDate")]
        
        public System.DateTime? EstimatedDeliveryDate { get; set; }
        
		        /// <summary>
        /// IsPaid
        /// </summary>
        [Display(Name = "IsPaid")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Boolean IsPaid  { get; set; }
        
		        /// <summary>
        /// ShippingFee
        /// </summary>
        [Display(Name = "ShippingFee")]
        [Required(ErrorMessage = "請輸入{0}")]
        public System.Int32 ShippingFee  { get; set; }
        
		        /// <summary>
        /// StoreName
        /// </summary>
        [Display(Name = "StoreName")]
        [StringLength(maximumLength:30,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String StoreName  { get; set; }
        
		        /// <summary>
        /// StoreType
        /// </summary>
        [Display(Name = "StoreType")]
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超過{1}字")]
		  public System.String StoreType  { get; set; }
        
		
    }
}

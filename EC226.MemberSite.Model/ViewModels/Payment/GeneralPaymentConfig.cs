using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC226.MemberSite.Model.ViewModels.Payment
{
   public class GeneralPaymentConfig
   {
      public int Sort { get; set; }
      public string PaymentCode { get; set; }
      public string ClientID { get; set; }
      public string Secret { get; set; }
      public string StoreID { get; set; }
      public string StoreName { get; set; }
      public string MerchantID { get; set; }
      public string PublicKeyID { get; set; }
      public string PrivateKey { get; set; }
      public string PublishableKey { get; set; }
      public string SecretKey { get; set; }
      public bool IsSandbox { get; set; }
      public string EndPoint { get; set; }
      public decimal? MinAmount { get; set; }
      public string AllowCountries { get; set; }
      public bool Disable { get; set; }      
      public bool Authorize { get; set; }
   }
}

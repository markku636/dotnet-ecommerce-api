using EC226.Payment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{
    public class CheckCartItem
    {
        [JsonProperty("spuId")]
        public int StandardProductUnitId { get; set; }

        [JsonProperty("skus")]
        public string Skus { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

    }

    public class 
        
        
        PaymentParameter
    {
        public string Description { get; set; }
        public Decimal Amount { get; set; }
        public String OrderNo { get; set; }

        public DateTime Date { get; set; }
        public List<EC226.Payment.PaymentItem> Items { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string PaymentOptionCode { get; set; }
        public string PaymentOptionSubCode { get; set; }

        public bool IsNeedSubmitButton { get; set; }

        public string ReceiverName { get; set; }

        public string ReceiverCellPhone { get; set; }       
        public string ReceiverEmail { get; set; }
        public string ReceiverStoreID { get; set; }

        public string ReturnStoreID { get; set; }


        public Dictionary<string,string> OtherOptions { get; set; }
    }

    public class PaymentData
    {
        public string HtmlPostForm { get; set; }
    }

    public class CheckCartResultVM
    {
        [JsonProperty("spuId")]
        public int StandardProductUnitId { get; set; }

        public string Skus { get; set; }

        public int AvableStock { get; set; } = 0;

        public bool IsEnable { get; set; }

        public bool IsEnoughStock { get; set; }
    }
}

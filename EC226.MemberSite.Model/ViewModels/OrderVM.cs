using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }

        [DisplayName("收件者")]
        public string RecipientName { get; set; }
        public string Phone { get; set; }

        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        
        [DisplayName("訂單購日期")]
        public DateTime Date { get; set; }

        [DisplayName("訂單號碼")]
        public string OrderNo { get; set; }

        [DisplayName("訂單狀態")]
        public int Status { get; set; }

        [DisplayName("訂單總金額")]
        public int FinalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public List<OrderDetailVM> OrderDetails { get; set; }
                
        public bool IsPaid { get; set; }
        public int ShippingFee { get; set; }        
    }

    public class OrderDetailVM
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int StandardProductUnitId { get; set; }
        public string Skus { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int SubTotal { get; set; }
        public string Memo { get; set; }
        public string SpuName { get; set; }
        public string SpecOptionName { get; set; }    
        public string CoverUrl { get; set; }
    }
}

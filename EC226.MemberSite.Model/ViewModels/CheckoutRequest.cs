using System;
using System.Collections.Generic;
using System.Text;

namespace EC226.MemberSite.Model.ViewModels
{
    public class CheckoutRequest
    {        
        public string RecipientName { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }       
        public int PaymentId { get; set; }
        public int PaymentOptionId { get; set; }


        public List<CheckCartItem> cartItems { get; set; }
    }
}

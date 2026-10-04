using EC226.Payment.Interfaces;

namespace EC226.Payment
{
    public class PaymentItem : IItem
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
namespace ECommerceWeb.Models
{
    public class Transaction
    { 
        public Guid TransactionId { get; set; }
        public string pi_Id { get; set; }
        public string PaymentStatus { get; set; }
        public Guid OrderId { get; set; }
        public Order order { get; set; }
    }
}

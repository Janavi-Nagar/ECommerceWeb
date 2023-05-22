using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceWeb.Models
{
    public class OrderItem
    {
        [Key]
        public Guid Id { get; set; }

        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Products Products { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}

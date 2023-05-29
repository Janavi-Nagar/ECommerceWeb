using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceWeb.Models
{
    public class OrderDetails
    {
        [Key]
        public Guid OrderDetailsId { get; set; }
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
    }
}

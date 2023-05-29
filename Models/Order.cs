using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }
        public DateTime OrderCreated_Date { get; set; }
        public int OrderStatusId { get; set; }
        public decimal Net { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GrossAmount { get; set; }
        
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}

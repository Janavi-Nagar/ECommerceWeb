using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerceWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace ECommerceWeb.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser UserName { get; set; }
        public Guid CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public List<Cart> Cart { get; set; }
        public int TotalPrice { get; set; }

        //public List<OrderItem> OrderItems { get; set; }
    }
}

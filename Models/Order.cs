using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ECommerceWeb.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string Email { get; set; }

        public string Id { get; set; }
        [ForeignKey(nameof(Id))]
        public IdentityUser UserName { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}

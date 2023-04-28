using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceWeb.Models
{
    public class Cart
    {
        [Key]
        public Guid CartId { get; set; }

        public int Quantity { get; set; }
        public int TotalPrice { get; set; }

        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Products Products { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}

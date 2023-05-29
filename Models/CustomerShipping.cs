using ECommerceWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    public class CustomerShipping
    {
        [Key]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Username { get; set; }
        
        [StringLength(70)]
        public string Address { get; set; }
        [StringLength(40)]
        public string City { get; set; }
        [StringLength(40)]
        public string State { get; set; }
        [StringLength(10)]
        public string Zip { get; set; }
        [StringLength(40)]
        public string Country { get; set; }
        [StringLength(24)]
        public string Phone { get; set; }
    }
}

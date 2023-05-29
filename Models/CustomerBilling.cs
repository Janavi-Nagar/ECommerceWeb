using ECommerceWeb.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    public class CustomerBilling
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(10)]
        public string FirstName { get; set; }
        [StringLength(10)]
        public string LastName { get; set; }

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
        public string OrderId { get; set; }
    }
}

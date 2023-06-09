using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [StringLength(70)]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required")]
        [StringLength(40)]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required")]
        [StringLength(40)]
        public string State { get; set; }
        [Required(ErrorMessage = "Postal Code is required")]
        [DisplayName("Postal Code")]
        [StringLength(10)]
        public string Zip { get; set; }
        [Required(ErrorMessage = "Country is required")]
        [StringLength(40)]
        public string Country { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Cart { get; set; }
        public int NoOfCartItems { get; set; }
        public decimal discountamount { get; set; }
        public decimal grossamount { get; set; }
        public decimal discount { get; set; }
        public decimal Net { get; set; }
        public string CartDetails { get; set; }
    }
}

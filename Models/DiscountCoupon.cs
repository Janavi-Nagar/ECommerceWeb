using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    public class DiscountCoupon
    {
        [Key]
        public Guid CouponId { get; set; }
        [Required(ErrorMessage = "Please enter CouponCode")]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "Discount Code must be between 4 and 10 char")]
        public string CouponCode { get; set; }
        [Required(ErrorMessage = "Please enter status")]
        public bool Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime Valid_Till { get; set; }
        [Required(ErrorMessage = "Please enter discount price")]
        public decimal Discount { get; set; }
    }
}

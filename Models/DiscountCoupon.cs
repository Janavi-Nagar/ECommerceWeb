using System.ComponentModel.DataAnnotations;

namespace ECommerceWeb.Models
{
    public class DiscountCoupon
    {
        [Key]
        public Guid CouponId { get; set; }
        [Required(ErrorMessage = "Please enter CouponCode")]
        [StringLength(10)]
        public string CouponCode { get; set; }
        [Required(ErrorMessage = "Please enter status")]
        public bool Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime Valid_Till { get; set; }
        [Required(ErrorMessage = "Please enter discount price")]
        public decimal Discount { get; set; }
    }
}

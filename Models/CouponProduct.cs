using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceWeb.Models
{
    public class CouponProduct
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Products Products { get; set; }
        public Guid CouponId { get; set; }
        [ForeignKey("CouponId")]
        public virtual DiscountCoupon DiscountCoupon { get; set; }
    }
}

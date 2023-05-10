using ECommerceWeb.Models;

namespace ECommerceWeb.Interface
{
    public interface ICouponService
    {
        List<DiscountCoupon> GetCoupon();
        DiscountCoupon GetCouponById(string CouponId);
        int AddUpdateCoupon(DiscountCoupon model);
        Task<bool> DeleteCoupon(Guid CouponId);
    }
}

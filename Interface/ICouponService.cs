using ECommerceWeb.Models;

namespace ECommerceWeb.Interface
{
    public interface ICouponService
    {
        List<DiscountCoupon> GetCoupon();
        List<Products> GetProductList();
        DiscountCoupon GetCouponById(string CouponId);
        Guid AddUpdateCoupon(DiscountCoupon model);
        void AddCouponProduct(string[] selected, Guid id);
        Task<bool> DeleteCoupon(string CouponId);
    }
}

using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface ICouponService
    {
        List<DiscountCoupon> GetCoupon();
        List<Products> GetProductList();
        CouponViewModel GetCouponById(string CouponId);
        void AddUpdateCoupon(CouponViewModel model);
        Task<bool> DeleteCoupon(string CouponId);
    }
}

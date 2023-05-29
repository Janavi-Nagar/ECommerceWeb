using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface ICouponService
    {
        IEnumerable<DiscountCoupon> GetCoupon();
        IQueryable<Guid> CouponProducts(Guid couponId);
        List<Products> GetProductList();
        CouponViewModel GetCouponById(string CouponId);
        void AddUpdateCoupon(CouponViewModel model);
        Task<bool> DeleteCoupon(string CouponId);
    }
}

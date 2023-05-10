using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Controllers
{
    public class DiscountCouponController : Controller
    {
        private readonly ICouponService couponService;

        public DiscountCouponController(ICouponService _couponService)
        {
            couponService = _couponService;
        }
        
        [HttpGet]
        public IActionResult DiscountCoupon()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DiscountCoupon(DiscountCoupon coupon)
        {
            return View();
        }
    }
}
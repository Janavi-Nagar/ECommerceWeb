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
            return View(couponService.GetCoupon());
        }

        [HttpGet]
        public async Task<IActionResult> GetCouponInfo(string Id)
        {            
            return PartialView("_AddCoupon", Id != "0" ? couponService.GetCouponById(Id) : new DiscountCoupon());
        }

        [HttpPost]
        public IActionResult DiscountCoupon(DiscountCoupon coupon)
        {
            if (ModelState.IsValid)
            {
                var result = couponService.AddUpdateCoupon(coupon);
                if (result == 1 | result == 2)
                {
                    return RedirectToAction("DiscountCoupon");
                }
                return RedirectToAction("DiscountCoupon");
            }
            return RedirectToAction("DiscountCoupon");
        }

        public async Task<IActionResult> DeleteCoupon(string Id)
        {
            await couponService.DeleteCoupon(Id);
            return RedirectToAction("DiscountCoupon");
        }
    }
}

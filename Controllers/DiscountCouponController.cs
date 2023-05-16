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
        public List<DiscountCoupon> Coupon()
        {
            return couponService.GetCoupon();
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
                return Json(result); // return type 
            }
            return RedirectToAction("GetCouponInfo", coupon);
        }

        public async Task<IActionResult> DeleteCoupon(string Id)
        {
            await couponService.DeleteCoupon(Id);
            return RedirectToAction("DiscountCoupon");
        }
    }
}

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
            var data = from prd in couponService.GetProductList()
                       select new
                       {
                           ProductId = prd.ProductId,
                           ProductName = prd.ProductName
                       };
            ViewBag.data = data;
            return PartialView("_AddCoupon", Id != "0" ? couponService.GetCouponById(Id) : new DiscountCoupon());
        }

        [HttpPost]
        public IActionResult DiscountCoupon(DiscountCoupon coupon, string[] selected)
        {
            if (ModelState.IsValid)
            {
                var result = couponService.AddUpdateCoupon(coupon);
                return Json(result); // return type 
            }
            return RedirectToAction("GetCouponInfo", coupon);
        }
        [HttpPost]
        public IActionResult CouponProduct(string[] selected, Guid id)
        {
            couponService.AddCouponProduct(selected, id);
            return RedirectToAction("DiscountCoupon");
        }

        public async Task<IActionResult> DeleteCoupon(string Id)
        {
            await couponService.DeleteCoupon(Id);
            return RedirectToAction("DiscountCoupon");
        }
    }
}

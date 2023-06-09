using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DiscountCouponController : Controller
    {
        private readonly ICouponService couponService;
        private readonly IProductService productService;

        public DiscountCouponController(ICouponService _couponService, IProductService _productService)
        {
            couponService = _couponService;
            productService = _productService;
        }
        
        [HttpGet]
        public IActionResult DiscountCoupon()
        {
            return View(couponService.GetCoupon());
        }
        [HttpGet]
        public IEnumerable<DiscountCoupon> Coupon()
        {
            return couponService.GetCoupon();
        }
        [HttpGet]
        public async Task<IActionResult> GetCouponInfo(string Id)
        {
            var data = from prd in productService.Products()
                       select new
                       {
                           ProductId = prd.ProductId,
                           ProductName = prd.ProductName
                       };
            ViewBag.data = data;
            return PartialView("_AddCoupon", Id != "0" ? couponService.GetCouponById(Id) : new CouponViewModel());
        }

        [HttpPost]
        public IActionResult DiscountCoupon(CouponViewModel coupon)
        {
            if (ModelState.IsValid)
            {
                couponService.AddUpdateCoupon(coupon);
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

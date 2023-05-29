using ECommerceWeb.Helpers;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace ECommerceWeb.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService cartService;
        private readonly ICheckoutService checkoutService;
        private readonly ICouponService couponService;

        public CheckoutController(ICartService _cartService, ICheckoutService _checkoutService, ICouponService _couponService)
        {
            cartService = _cartService;
            checkoutService = _checkoutService;
            couponService = _couponService;
        }
        public ActionResult Checkout()
        {
            return View(new CheckoutViewModel());
        }

        public ActionResult GetCartProduct()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            CheckoutViewModel checkoutView = new CheckoutViewModel();
            checkoutView.Cart = cartService.GetCartByUserId(userId);
            checkoutView.NoOfCartItems = checkoutView.Cart.Count();
            return PartialView("_CartProducts", checkoutView);
        }

        public ActionResult ValidateCouponCode(string code)
        {

            var Coupons = couponService.GetCoupon().FirstOrDefault(m => m.CouponCode == code);
            if (Coupons != null)
            {
                var Product = couponService.CouponProducts(Coupons.CouponId).ToList();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var Cart = cartService.GetCartByUserId(userId).ToList();
                var model = new
                {
                    Coupon = Coupons,
                    cartitem = Cart.Where(x => Product.Contains(x.Products.ProductId))
                };
                return Json(model); // 1 for validate and 0 for not
            }
            return Json(null);
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(CheckoutViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.UserId = userId;
            await checkoutService.AddUpdateAddress(model);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", new List<CartItem>());
            return RedirectToAction("Index","Home");
        }
    }
}

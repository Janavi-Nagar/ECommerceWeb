using ECommerceWeb.Data;
using ECommerceWeb.Helpers;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceWeb.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly UserManager<IdentityUser> userManager;

        public CartController(IProductService productService, ICartService cartService, UserManager<IdentityUser> UserManager)
        {
            _productService = productService;
            _cartService = cartService;
            userManager = UserManager;
        }
       
        [Route("cart")]
        public IActionResult Cart()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int no = 0;
            if(cart != null)
            {
                no = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart").Count();
                ViewBag.cart = cart;
                ViewBag.total = cart.Sum(item => item.Products.Price * item.Quantity);
            }
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var UserCart = _cartService.GetCartByUserId(userId);
                ViewBag.cart = UserCart;
                ViewBag.total = UserCart.Sum(item => item.Products.Price * item.Quantity);
            }
           
            return View();
        }

        //[Route("Buy/{Pid}")]
        [HttpGet]
        public IActionResult Buy(string Pid)
        {
            var ProductId = new Guid(Pid);
            var data = _productService.GetProductById(ProductId);
            List<CartItem> cart = new List<CartItem>();
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                cart.Add(new CartItem { Products = data, Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    Cart model = new Cart();
                    model.ProductId = ProductId;
                    model.Quantity = 1;
                    model.TotalPrice = model.Quantity * data.Price;
                    model.UserId = userId;
                    var ret = _cartService.SaveCartProduct(model);
                }
            }
            else
            {
                cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                string proid = ProductId.ToString();
                int index = isExist(proid);
                if (index != 1)
                {
                    cart[index].Quantity++;
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        Cart model = new Cart();
                        model.ProductId = ProductId;
                        model.UserId = userId;
                        var ret = _cartService.SaveCartProduct(model);
                    }
                }
                else
                {
                    cart.Add(new CartItem { Products = data, Quantity = 1 });
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        Cart model = new Cart();
                        model.ProductId = ProductId;
                        model.Quantity = 1;
                        model.TotalPrice = model.Quantity * data.Price;
                        model.UserId = userId;
                        var ret = _cartService.SaveCartProduct(model);
                    }
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
           // return RedirectToAction("Index", "Home");
            return Json(cart.Count());
        }

        [Route("remove/{ProductId}")]
        public IActionResult Remove(Guid ProductId)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            string proid = ProductId.ToString();
            int index = isExist(proid);
            
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Cart model = new Cart();
                model.UserId = userId;
                model.ProductId = ProductId;
                var ret = _cartService.DeleteCartProduct(model);
            }
            return RedirectToAction("Cart", "Cart");
        }

        private int isExist(string proid)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                string data = cart[i].Products.ProductId.ToString();
                if (data.Equals(proid))
                {
                    return i;
                }
            }
            return 1;
        }

    }
}

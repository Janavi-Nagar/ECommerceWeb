using ECommerceWeb.Data;
using ECommerceWeb.Helpers;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly UserDbContext DbContext;

        public CartController(UserDbContext userDbContext)
        {
            DbContext = userDbContext;
        }

        [Route("cart")]
        public IActionResult Cart()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int no = 0;
            if(cart != null)
            {
                no = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart").Count();
                //ViewBag.no = no;
            }
            
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Products.Price * item.Quantity);
            
            return View();
        }

        [Route("buy/{ProductId}")]
        public IActionResult Buy(Guid ProductId)
        
        
        {
            // Add to cart

            ProductViewModel productModel = new ProductViewModel();
            object result = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            var data = DbContext.Products.FirstOrDefault(m => m.ProductId == ProductId);
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                List<CartItem> cart = new List<CartItem>();
                cart.Add(new CartItem { Products = data, Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                string proid = ProductId.ToString();
                int index = isExist(proid);
                if (index != 1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Products = data, Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Cart");
        }

        [Route("remove/{ProductId}")]
        public IActionResult Remove(Guid ProductId)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            string proid = ProductId.ToString();
            int index = isExist(proid);
            
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
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

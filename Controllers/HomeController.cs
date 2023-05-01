using ECommerceWeb.Data;
using ECommerceWeb.Helpers;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ECommerceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDbContext dbContext;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(ILogger<HomeController> logger, UserDbContext context, IProductService productService, ICartService cartService)
        {
            _logger = logger;
             dbContext = context;
            _productService = productService;
            _cartService = cartService;
        }

       public int NoOfCartProduct()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int no = 0;
            if (cart != null)
            {
                no = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart").Count();
             }
            return no;
        }
       
        public async Task<IActionResult> Index()
        {

            //for session data add to database
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartBeforeLogin = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                var oldRecord = _cartService.GetCartByUserId(userId);
                if (oldRecord != null)
                {
                    List<CartItem> cart = new List<CartItem>();
                    foreach (var item in oldRecord)
                    {
                        cart.Add(new CartItem { Products = item.Products, Quantity = item.Quantity });
                    }
                    foreach(var item in cartBeforeLogin) 
                    {
                        cart.Add(new CartItem { Products = item.Products, Quantity = item.Quantity });
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                if (cartBeforeLogin != null)
                {
                    foreach (var item in cartBeforeLogin)
                    {
                        var data = _productService.GetProductById(item.Products.ProductId);
                        Cart model = new Cart();
                        model.ProductId = item.Products.ProductId;
                        model.Quantity = item.Quantity;
                        model.TotalPrice = model.Quantity * data.Price;
                        model.UserId = userId;
                        _cartService.SaveCartProduct(model);
                    }
                }
            }

            var product = await _productService.GetProducts();
            return View(product);
        }
       
        [HttpGet]
        public async Task<IActionResult> Details(string productId)
        {
            ViewBag.Productcategory = await _productService.GetProductCategory();
            return View("Details", await _productService.GetProductInfo(productId));
        }

        public IActionResult Privacy()
        {
            return View();
        }
       
       



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
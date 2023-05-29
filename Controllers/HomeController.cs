using ECommerceWeb.Data;
using ECommerceWeb.Helpers;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ECommerceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        public HomeController(ILogger<HomeController> logger, IHomeService homeService, ICartService cartService, IProductService productService)
        {
            _logger = logger;
            _homeService = homeService;
            _cartService = cartService;
            _productService = productService;
        }

        public int NoOfCartProduct()
        {
            int no = 0;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cart = _cartService.GetCartByUserId(userId);
                if (cart != null)
                {
                    no = cart.Count();
                }
            }
            else
            {
                var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                if (cart != null)
                {
                    no = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart").Count();
                }
            }
            return no;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetProduct(int currentPageIndex = 1, string searchtext = "")
        {
            return PartialView("_Product", this.GetProducts(currentPageIndex, searchtext));
        }


        private ProductModel GetProducts(int currentPageIndex, string searchtext)
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
                    if (cartBeforeLogin != null)
                    {
                        foreach (var item in cartBeforeLogin)
                        {
                            var cntcartdata = cart.Select(x => x.Products.ProductId).Where(x => x.Equals(item.Products.ProductId)).Count();
                            if (cntcartdata == 0)
                                cart.Add(new CartItem { Products = item.Products, Quantity = item.Quantity });
                        }
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
            ProductSearchViewModel searchmodel = new ProductSearchViewModel();
            searchmodel.searchtext = searchtext;
            searchmodel.pagesize = 5;
            searchmodel.pagenumber = currentPageIndex == 0 ? 1 : currentPageIndex;
            var product = _homeService.ProductSearch(searchmodel);
            return product;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string productId)
        {
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
using ECommerceWeb.Helpers;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
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
        private readonly IReCaptchaService _reCaptchaService;
        
        public HomeController(ILogger<HomeController> logger, IHomeService homeService, ICartService cartService, IProductService productService, IReCaptchaService reCaptchaService)
        {
            _logger = logger;
            _homeService = homeService;
            _cartService = cartService;
            _productService = productService;
            _reCaptchaService = reCaptchaService;
        }

        public async Task<int> NoOfCartProduct()
        {
            int no = 0;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cart = await _cartService.GetCartByUserId(userId);
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
            //Cart cart = new Cart();
            //cart.Quantity = 10;
            //var mail = new HtmlTemplate<Cart>("Sample", cart);
            //var htmlstr = mail.GenerateBody();
            
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
                    foreach (var item in oldRecord.Result)
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
        [HttpGet]
        [ValidateGoogleCaptcha]
        public async Task<JsonResult> Verifyforsignup(string token)
        {
            return Json("success");
        }
        [HttpGet]
        public async Task<JsonResult> Verify(string token)
        {
            var verified = await _reCaptchaService.Varification(token);
            return Json(verified);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
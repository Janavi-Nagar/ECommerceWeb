using ECommerceWeb.Data;
using ECommerceWeb.Helpers;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerceWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDbContext dbContext;
        private readonly IProductService _productService;
        public HomeController(ILogger<HomeController> logger, UserDbContext context, IProductService productService)
        {
            _logger = logger;
             dbContext = context;
            _productService = productService;
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
            try { 
            var product = await _productService.GetProducts();
                ViewBag.Productcategory = await _productService.GetProductCategory();
            return View(product);
            }
            catch(Exception ex)
            {
                _logger.LogError(string.Format("Excellent description goes here about the exception. Happened for client"), ex);
                return View(Privacy);
            }
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
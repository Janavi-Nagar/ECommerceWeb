using ECommerceWeb.Data;
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

       
        public async Task<IActionResult> Index()
        {
            var product = await _productService.GetProducts();
            ViewBag.Productcategory = await _productService.GetProductCategory();
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string proid)
        {
            ViewBag.Productcategory = await _productService.GetProductCategory();
            return View("Details", await _productService.GetProducyInfo(proid));
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
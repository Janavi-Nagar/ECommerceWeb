using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
        }
        [Authorize(Roles = "Admin, Seller")]
        public async Task<IActionResult> Product()
        {
            if (User.IsInRole("Admin"))
            {
                var product = await _productService.GetProducts();
                return View(product);
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var product = await _productService.SellerProducts(userId);
                return View(product);
            }
        }

        [Authorize(Roles = "Admin, Seller")]
        [HttpGet]
        public async Task<IActionResult> ProductForm()
        {
            ViewBag.Productcategory = _productCategoryService.GetProductCategory();
            return View(new ProductViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetProductInfo(string productId)
        {
            ViewBag.Productcategory =  _productCategoryService.GetProductCategory();
            return View("ProductForm", await _productService.GetProductInfo(productId));
        }
        [HttpPost]
        public async Task<IActionResult> ProductForm(ProductViewModel model)
        {
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                        var ret = await _productService.SaveProductData(model, userId);
                        if (ret == 1 || ret == 2)
                        {
                            return Redirect("Product");
                        }
                        else
                            return View("ProductForm", model);
                    }
                }
                else
                {
                    return View("ProductForm", model);
                }
            }
            return View("ProductForm", model);
        }

        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            await _productService.DeleteProduct(productId);
            return Redirect("Product");
        }
    }
}

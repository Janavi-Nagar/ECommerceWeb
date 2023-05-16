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

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [Authorize(Roles = "Admin, Seller")]
        public async Task<IActionResult> Product()
        {
            if (User.IsInRole("Admin"))
            {
                var product = await _productService.GetProducts();
                var data = await _productService.GetProductCategory();
                var output = from prt in product
                             join dt in data
            
                             on prt.ProductCategoryId equals dt.ProductCategoryId
                             select new Products
                             {
                                 ProductName = prt.ProductName,
                                 ProductPicture = prt.ProductPicture,
                                 Price = prt.Price,
                                 InStock = prt.InStock,
                                 ProductCategory = dt
                             };
                return View(output);
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var product = await _productService.SellerProducts(userId);
                var data = await _productService.GetProductCategory();
                var output = from prt in product
                             join dt in data
                             on prt.ProductCategoryId equals dt.ProductCategoryId
                             select new Products
                             {
                                 ProductName = prt.ProductName,
                                 ProductPicture = prt.ProductPicture,
                                 Price = prt.Price,
                                 InStock = prt.InStock,
                                 ProductCategory = dt
                             };

                return View(output);
            }
        }

        [Authorize(Roles = "Admin, Seller")]
        [HttpGet]
        public async Task<IActionResult> ProductForm()
        {
            ViewBag.Productcategory = await _productService.GetProductCategory();
            return View(new ProductViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetProductInfo(string productId)
        {
            ViewBag.Productcategory = await _productService.GetProductCategory();
            return View("ProductForm", await _productService.GetProductInfo(productId));
        }
        [HttpPost]
        public async Task<IActionResult> ProductForm(ProductViewModel model)
        {
            if (model != null)
            {
                if (ModelState.IsValid)
                {
                    var ret = await _productService.SaveProductData(model);
                    if (ret == 1 || ret == 2)
                    {
                        return Redirect("Product");
                    }
                    else
                        return View("ProductForm", model);
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

using ECommerceWeb.Interface;
using ECommerceWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Product()
        {
            var product = await _productService.GetProducts();
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> ProductForm()
        {
            ViewBag.Productcategory = await _productService.GetProductCategory();
            return View(new ProductViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> GetProductInfo(string proid)
        {
            ViewBag.Productcategory = await _productService.GetProductCategory();
            return View("ProductForm", await _productService.GetProducyInfo(proid));
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
                        return Redirect("Product");
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

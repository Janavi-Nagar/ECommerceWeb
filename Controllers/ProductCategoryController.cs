using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategory )
        {
            productCategoryService = productCategory;
        }
        public IActionResult ProductCategoryList()
        {
            var product = productCategoryService.GetProductCategory();
            return View(product);
        }
        [HttpGet]
        public IActionResult ProductCategoryForm()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetProductInfo(string proid)
        {
            return View("ProductCategoryForm", productCategoryService.ProCategoryInfo(proid));
        }
        [HttpPost]
        public IActionResult ProductCategoryForm(ProductCategory model)
        {
            if (model != null)
            {
                //if (ModelState.IsValid)
                //{
                    var ret = productCategoryService.SaveProductCategory(model);
                    if (ret == 1 || ret == 2)
                        return Redirect("ProductCategoryList");
                    else
                        return View("ProductCategoryForm", model);
                //}
                //else
                //{
                //    return View("ProductCategoryForm", model);
                //}
            }
            return View("ProductForm", model);
        }
        public async Task<IActionResult> DeleteProCategory(Guid productcategoryId)
        {
            await productCategoryService.DeleteProCategory(productcategoryId);
            return Redirect("ProductCategoryList");
        }
    }
}

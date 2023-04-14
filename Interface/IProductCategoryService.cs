using ECommerceWeb.Models;

namespace ECommerceWeb.Interface
{
    public interface IProductCategoryService
    {
       
            List<ProductCategory> GetProductCategory();
            ProductCategory ProCategoryInfo(string productcategoryid);
            int SaveProductCategory(ProductCategory model);
            Task<bool> DeleteProCategory(Guid productcategoryId);
       
    }
}

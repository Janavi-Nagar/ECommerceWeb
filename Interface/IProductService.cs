using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface IProductService
    {
        Task<List<Products>> GetProducts();
        Task<List<ProductCategory>> GetProductCategory();
        Products GetProductById(Guid ProductId);
        Task<ProductViewModel> GetProductInfo(string productId);
        Task<int> SaveProductData(ProductViewModel model);
        Task<bool> DeleteProduct(Guid productId);

    }
}

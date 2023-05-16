using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface IProductService
    {
        Task<List<Products>> GetProducts();
        Task<List<Products>> SellerProducts(string userId);
        Task<List<ProductCategory>> GetProductCategory();
        Task<ProductViewModel> GetProductInfo(string productId);
        Task<int> SaveProductData(ProductViewModel model);
        Task<bool> DeleteProduct(Guid productId);
        Products GetProductById(Guid ProductId);
    }
}

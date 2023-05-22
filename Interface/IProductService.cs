using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface IProductService
    {
        List<Products> Products();
        Task<IQueryable<ProductViewModel>> GetProducts();
        Task<List<Products>> SellerProducts(string userId);
        Task<ProductViewModel> GetProductInfo(string productId);
        Task<int> SaveProductData(ProductViewModel model);
        Task<bool> DeleteProduct(Guid productId);
        Products GetProductById(Guid ProductId);
    }
}

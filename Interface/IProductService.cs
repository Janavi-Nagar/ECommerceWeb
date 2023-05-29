using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface IProductService
    {
        List<Products> Products();
        Task<IQueryable<ProductViewModel>> GetProducts();
        Task<IQueryable<ProductViewModel>> SellerProducts(string userId);
        Task<ProductViewModel> GetProductInfo(string productId);
        Task<int> SaveProductData(ProductViewModel model, string userId);
        Task<bool> DeleteProduct(Guid productId);
        Products GetProductById(Guid ProductId);
    }
}

using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface IProductService
    {
        Task<List<Products>> GetProducts();
        Task<List<ProductCategory>> GetProductCategory();
        Task<ProductViewModel> GetProducyInfo(string prductid);
        Task<int> SaveProductData(ProductViewModel model);
        Task<bool> DeleteProduct(Guid productId);

    }
}

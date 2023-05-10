using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface IHomeService
    {
        ProductParameters ProductSearch(ProductSearchViewModel searchmodel);

        Products GetProductById(Guid ProductId);

        Task<ProductParameters> IndexProducts(int currentPageIndex);
    }
}

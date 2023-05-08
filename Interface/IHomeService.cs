using ECommerceWeb.Models;

namespace ECommerceWeb.Interface
{
    public interface IHomeService
    {
        ProductParameters ProductSearch(string search);

        Products GetProductById(Guid ProductId);

        Task<ProductParameters> IndexProducts(int currentPageIndex);
    }
}

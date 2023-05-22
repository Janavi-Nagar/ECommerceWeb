using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface IHomeService
    {
        ProductModel ProductSearch(ProductSearchViewModel searchmodel);

    }
}

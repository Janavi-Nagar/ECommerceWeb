using ECommerceWeb.Models;

namespace ECommerceWeb.Interface
{
    public interface ICartService
    {
        Task<List<Cart>> GetCartProducts();
        Products GetProductById(Guid ProductId);
        Task<List<CartItem>> GetCartByUserId(string UserId);
        int SaveCartProduct(Cart cart);
        Task<bool> DeleteCartProduct(Cart model);
    }
}

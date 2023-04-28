using ECommerceWeb.Models;

namespace ECommerceWeb.Interface
{
    public interface ICartService
    {
        Task<List<Cart>> GetCartProducts();
        IQueryable<CartItem> GetCartByUserId(string UserId);
        int SaveCartProduct(Cart cart);
        Task<bool> DeleteCartProduct(Cart model);
    }
}

using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWeb.Service
{
    public class CartService : ICartService
    {
        private readonly UserDbContext dbContext;
        public CartService(UserDbContext context)
        {
            dbContext = context;
        }
        public Products GetProductById(Guid ProductId)
        {
            var product = dbContext.Products.FirstOrDefault(m => m.ProductId == ProductId);
            return product;
        }
        public async Task<List<Cart>> GetCartProducts()
        {
            return await dbContext.Cart.ToListAsync();
        }
        
        public IQueryable<CartItem> GetCartByUserId(string UserId)
        {
            var data = from crt in dbContext.Cart
                       join prd in dbContext.Products
                       on crt.ProductId equals prd.ProductId
                       where crt.UserId == UserId
                       select new CartItem
                            {
                                Quantity = crt.Quantity,
                                Products = prd
                            };
            return data;
        }


        public int SaveCartProduct(Cart model)
        {
            var retresult = 0;
            if (!dbContext.Cart.Any(m => m.ProductId == model.ProductId && m.UserId == model.UserId))
            {
                dbContext.Cart.Add(model);
                dbContext.SaveChanges();
                retresult = 1;
            }
            else
            {
                var cart = dbContext.Cart.FirstOrDefault(m => m.ProductId == model.ProductId && m.UserId == model.UserId);
                if (cart != null)
                {
                    var data = dbContext.Products.Find(cart.ProductId);
                    cart.Quantity = model.Quantity;
                    cart.TotalPrice = data.Price * model.Quantity;
                    dbContext.Cart.Update(cart);
                    dbContext.SaveChanges();
                    retresult = 2;
                }
            }
            return retresult;
        }
        
        public async Task<bool> DeleteCartProduct(Cart model)
        {
            var data = dbContext.Cart
                        .Where(m => m.ProductId == model.ProductId)
                        .Where(p => p.UserId == model.UserId)
                        .FirstOrDefault();
            
            dbContext.Cart.Remove(data);
            dbContext.SaveChanges();
            
            return true;
        }

    }
}

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
            if (!(dbContext.Cart.Any(m => m.ProductId == model.ProductId)&& dbContext.Cart.Any(m => m.UserId == model.UserId)))
            {
                dbContext.Cart.Add(model);
                dbContext.SaveChanges();
                retresult = 1;
            }
            else
            {
                var cart = dbContext.Cart.FirstOrDefault(m => m.ProductId == model.ProductId);
                var data = dbContext.Products.Find(cart.ProductId);
                if (cart != null)
                {
                    cart.Quantity = cart.Quantity + 1;
                    cart.TotalPrice = cart.TotalPrice + data.Price;
                    dbContext.Cart.Update(cart);
                    dbContext.SaveChanges();
                    retresult = 2;
                }
            }
            return retresult;
        }
        
        public async Task<bool> DeleteCartProduct(Cart model)
        {
            var data = dbContext.Cart.FirstOrDefault(m => m.ProductId == model.ProductId);
            var data2 = dbContext.Cart.FirstOrDefault(m => m.UserId == model.UserId);
            if(data == data2)
            {
                var cart = await dbContext.Cart.FindAsync(data.CartId);
                dbContext.Cart.Remove(cart);
                dbContext.SaveChanges();
            }
            return true;
        }

    }
}

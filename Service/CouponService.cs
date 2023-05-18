using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ECommerceWeb.Service
{
    public class CouponService : ICouponService
    {
        private readonly UserDbContext dbContext;
        public CouponService(UserDbContext context)
        {
            dbContext = context;
        }
        public List<DiscountCoupon> GetCoupon()
        {
            return dbContext.DiscountCoupon.ToList();
        }
        public List<Products> GetProductList()
        {
            return dbContext.Products.ToList();
        }

        public DiscountCoupon GetCouponById(string CouponId)
        {
            var id = new Guid(CouponId);
            DiscountCoupon coupon = new DiscountCoupon();
            var data = dbContext.DiscountCoupon.FirstOrDefault(m => m.CouponId == id);
            if (data != null)
            {
                coupon.CouponId = data.CouponId;
                coupon.CouponCode = data.CouponCode;
                coupon.Valid_Till = data.Valid_Till;
                coupon.Discount = data.Discount;
                return coupon;
            }
            return coupon;
        }
        public Guid AddUpdateCoupon(DiscountCoupon model)
        {
            Guid retresult = Guid.Empty;
            if (model.CouponId == new Guid())
            {
                DiscountCoupon coupon = new DiscountCoupon
                {
                    CouponCode = model.CouponCode,
                    Valid_Till = model.Valid_Till,
                    Discount = model.Discount
                };
                dbContext.DiscountCoupon.Add(coupon);
                dbContext.SaveChanges();
                var data = dbContext.DiscountCoupon.FirstOrDefault(m => m.CouponCode == model.CouponCode);
                retresult = data.CouponId;
                    
            }
            else
            {
                var data = dbContext.DiscountCoupon.FirstOrDefault(m => m.CouponId == model.CouponId);
                if (data != null)
                {
                    data.CouponId = model.CouponId;
                    data.CouponCode = model.CouponCode;
                    data.Valid_Till = model.Valid_Till;
                    data.Discount = model.Discount;
                    dbContext.DiscountCoupon.Update(data);
                    dbContext.SaveChanges();
                    retresult = data.CouponId;
                }
            }
            return retresult;
        }

        public void AddCouponProduct(string[] selected, Guid id)
        {
            foreach(var i in selected)
            {
                var Id = new Guid(i);
                CouponProduct data = new CouponProduct
                {
                    ProductId = Id,
                    CouponId = id
                };
                dbContext.CouponProduct.Add(data);
                dbContext.SaveChanges();
            }
        }
        public async Task<bool> DeleteCoupon(string CouponId)
        {
            var id = new Guid(CouponId);
            var coupon = await dbContext.DiscountCoupon.FindAsync(id);
            dbContext.DiscountCoupon.Remove(coupon);
            dbContext.SaveChanges();
            return await Task.FromResult(true);
        }
    }
}



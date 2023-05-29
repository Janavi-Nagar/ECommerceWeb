using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
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
        public IEnumerable<DiscountCoupon> GetCoupon()
        {
            return dbContext.DiscountCoupon.ToList();
        }
        public List<Products> GetProductList()
        {
            return dbContext.Products.ToList();
        }
        public IQueryable<Guid> CouponProducts(Guid couponId)
        {
            return dbContext.CouponProduct
                        .Where(m => m.CouponId == couponId)
                        .Select(m => m.ProductId);
        }
        public CouponViewModel GetCouponById(string CouponId)
        {
            var id = new Guid(CouponId);
            CouponViewModel coupon = new CouponViewModel();
            var data = dbContext.DiscountCoupon.FirstOrDefault(m => m.CouponId == id);
            var data2 = dbContext.CouponProduct
                            .Where(m => m.CouponId == id)
                            .Select(u => u.ProductId)
                            .ToList();
            if (data != null)
            {
                coupon.CouponId = data.CouponId;
                coupon.CouponCode = data.CouponCode;
                coupon.Valid_Till = data.Valid_Till;
                coupon.Discount = data.Discount;
                coupon.ProductIds = data2;
                return coupon;
            }
            return coupon;
        }
        public void AddUpdateCoupon(CouponViewModel model)
        {
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
                var Coupon = dbContext.DiscountCoupon.FirstOrDefault(m => m.CouponCode == model.CouponCode);
                List<CouponProduct> data = new List<CouponProduct>();
                foreach (var i in model.ProductIds)
                {
                    data.Add(new CouponProduct { ProductId = i, CouponId = Coupon.CouponId });
                }
                dbContext.CouponProduct.AddRange(data);
                dbContext.SaveChanges();
            }
            else
            {
                var data = dbContext.DiscountCoupon.FirstOrDefault(m => m.CouponId == model.CouponId);
                var data2 = dbContext.CouponProduct
                                    .Where(m => m.CouponId == model.CouponId)
                                    .ToList();
                if (data != null)
                {
                    if (data2 != null)
                    {
                        dbContext.CouponProduct.RemoveRange(data2);
                        dbContext.SaveChanges();
                    }
                    data.CouponId = model.CouponId;
                    data.CouponCode = model.CouponCode;
                    data.Valid_Till = model.Valid_Till;
                    data.Discount = model.Discount;
                    dbContext.DiscountCoupon.Update(data);
                    dbContext.SaveChanges();
                    List<CouponProduct> cp = new List<CouponProduct>();
                    foreach (var i in model.ProductIds)
                    {
                        cp.Add(new CouponProduct { CouponId = data.CouponId, ProductId = i });
                    }
                    dbContext.CouponProduct.AddRange(cp);
                    dbContext.SaveChanges();
                }
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



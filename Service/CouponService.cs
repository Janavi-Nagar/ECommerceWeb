using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;

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

        public DiscountCoupon GetCouponById(string CouponId)
        {
            throw new NotImplementedException();
        }
        public int AddUpdateCoupon(DiscountCoupon model)
        {
            var retresult = 0;
            if (model.CouponId == new Guid())
            {
                DiscountCoupon coupon = new DiscountCoupon
                {
                    CouponCode = model.CouponCode,
                    Status = model.Status,
                    Date = model.Date,
                    Valid_Till = model.Valid_Till,
                    Discount = model.Discount
                };
                dbContext.DiscountCoupon.Add(coupon);
                dbContext.SaveChanges();
                retresult = 1;
            }
            else
            {
                var data = dbContext.DiscountCoupon.FirstOrDefault(m => m.CouponId == model.CouponId);
                if (data != null)
                {
                    data.CouponCode = model.CouponCode;
                    data.Status = model.Status;
                    data.Date = model.Date;
                    data.Valid_Till = model.Valid_Till;
                    data.Discount = model.Discount;
                    dbContext.DiscountCoupon.Update(data);
                    dbContext.SaveChanges();
                    retresult = 2;
                }
            }
            return retresult;
        }

        public async Task<bool> DeleteCoupon(Guid CouponId)
        {
            var coupon = await dbContext.DiscountCoupon.FindAsync(CouponId);
            dbContext.DiscountCoupon.Remove(coupon);
            dbContext.SaveChanges();
            return await Task.FromResult(true);
        }
    }
}



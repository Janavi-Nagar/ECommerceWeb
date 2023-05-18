namespace ECommerceWeb.Models.ViewModels
{
    public class CouponViewModel
    {
        public Guid CouponId { get; set; }
        public string CouponCode { get; set; }
        public DateTime Valid_Till { get; set; }
        public decimal Discount { get; set; }
        public List<Guid> ProductIds { get; set; }
    }
}

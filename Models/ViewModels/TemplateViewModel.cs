using ECommerceWeb.Areas.Identity.Data;

namespace ECommerceWeb.Models.ViewModels
{
    public class TemplateViewModel
    {
        public Order Order { get; set; }
        public List<OrderDetails> OrderDetailsList { get; set; }
        public CustomerBilling BillingAddress { get; set; }
    }
}

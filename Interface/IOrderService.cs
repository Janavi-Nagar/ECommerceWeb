using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface IOrderService
    {
        List<Order> OrderList(string Id, string role);
        TemplateViewModel GetTempleteData(string Id, string userId);
    }
}

using ECommerceWeb.Models;

namespace ECommerceWeb.Interface
{
    public interface IUserService
    {
         List<CustomerBilling> GetAddById(string Id);
    }
}

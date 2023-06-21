using ECommerceWeb.Areas.Identity.Data;
using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerceWeb.Service
{
    public class UserService : IUserService
    {
        private readonly UserDbContext dbContext;
        public UserService(UserDbContext context)
        {
            dbContext = context;
        }

        public List<CustomerBilling> GetAddById(string Id)
        {
            return dbContext.CustomerBilling
                        .Where(m => m.UserId == Id).ToList();
        }
    }
}

using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Migrations;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ECommerceWeb.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly UserDbContext dbContext;
        public CheckoutService(UserDbContext context)
        {
            dbContext = context;
        }
        public async Task<Guid> AddUpdateAddress(CheckoutViewModel model)
        {
            // Order data
            Models.Order order = new Models.Order
            {
                OrderCreated_Date = DateTime.Now,
                OrderStatusId = 0,
                Net = model.Net,
                DiscountAmount = model.discountamount,
                GrossAmount = model.grossamount,
                UserId = model.UserId,
            };
            dbContext.Order.Add(order);
            dbContext.SaveChanges();

            // Customer Billing address Details
            CustomerBilling Address = new CustomerBilling
            {
                Address = model.Address,
                City = model.City,
                State = model.State,
                Zip = model.Zip,
                Country = model.Country,
                OrderId = order.OrderId.ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            dbContext.CustomerBilling.Add(Address);
            dbContext.SaveChanges();

            // Order Details data
            dynamic myObject = JValue.Parse(model.CartDetails);
            List<OrderDetails> orderDetails = new List<OrderDetails>();
            foreach (var item in myObject)
            {
                OrderDetails orderDetails1 = new OrderDetails();
                orderDetails1.OrderId = order.OrderId;
                orderDetails1.ProductId = item.ProductId;
                orderDetails1.UnitPrice = item.UnitPrice;
                orderDetails1.Quantity = item.Quantity;
                orderDetails1.Discount = item.Discount;
                orderDetails.Add(orderDetails1);
            }
            dbContext.OrderDetails.AddRange(orderDetails);
            dbContext.SaveChanges();

            // delete cart product
            var cartdata = dbContext.Cart.Where(x => orderDetails.Select(x => x.ProductId).Contains(x.ProductId) && x.UserId == model.UserId).ToList();
            dbContext.Cart.RemoveRange(cartdata);
            dbContext.SaveChanges();

            return await Task.FromResult(order.OrderId);
        }

    }
}

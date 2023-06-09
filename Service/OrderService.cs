using ECommerceWeb.Areas.Identity.Data;
using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ECommerceWeb.Service
{
    public class OrderService : IOrderService
    {
        private readonly UserDbContext dbContext;
        private const string Template = "~/Views/Order/Template.cshtml";
        public OrderService(UserDbContext context)
        {
            dbContext = context;
        }
        public TemplateViewModel GetTempleteData(string Id, string userId)
        {
            var id = new Guid(Id);
            TemplateViewModel model = new TemplateViewModel();
            model.Order = dbContext.Order.FirstOrDefault(m => m.OrderId == id);
            model.BillingAddress = dbContext.CustomerBilling.FirstOrDefault(m => m.OrderId == model.Order.OrderId);
            model.OrderDetailsList = (from det in dbContext.OrderDetails
                                      join prd in dbContext.Products
                                      on det.ProductId equals prd.ProductId
                                      where det.OrderId == id
                                      select new OrderDetails
                                      {
                                          Products = prd,
                                          UnitPrice = det.UnitPrice,
                                          Quantity = det.Quantity,
                                          Discount = det.Discount
                                      }).ToList<OrderDetails>();
            return model;
        }
        public List<Order> OrderList(string Id, string role)
        {
            var builder = WebApplication.CreateBuilder();
            string conStr = builder.Configuration.GetConnectionString("UserDbContextConnection");
            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("DisplayOrders", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if(role != "Admin")
            {
                cmd.Parameters.AddWithValue("Id", Id);
            }
            con.Open();

            var adapt = new SqlDataAdapter();
            adapt.SelectCommand = cmd;
            var dataset = new DataSet();
            adapt.Fill(dataset);
            List<Order> Orders = new List<Order>();
            for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
            {
                Order order = new Order()
                {
                    OrderId = new Guid(dataset.Tables[0].Rows[i]["OrderId"].ToString()),
                    OrderCreated_Date = Convert.ToDateTime(dataset.Tables[0].Rows[i]["OrderCreated_Date"]),
                    OrderStatusId = Convert.ToInt16(dataset.Tables[0].Rows[i]["OrderStatusId"]),
                    GrossAmount = Convert.ToDecimal(dataset.Tables[0].Rows[i]["GrossAmount"]),
                    UserId = dataset.Tables[0].Rows[i]["UserId"].ToString()
                };
                var data = dbContext.ApplicationUsers.FirstOrDefault(m => m.Id == order.UserId);
                order.User = data;
                Orders.Add(order);
            }
            con.Close();
            return Orders;
        }
    }
}

using ECommerceWeb.Areas.Identity.Data;
using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Razor.Templating.Core;
using System.Security.Claims;
using System.Text;

namespace ECommerceWeb.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService OrderService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserDbContext dbContext;
        public OrderController(IOrderService _OrderService, RoleManager<IdentityRole> roleManager, UserDbContext _dbContext)
        {
            OrderService = _OrderService;
            this.roleManager = roleManager;
            dbContext = _dbContext;
        }
        public async Task<IActionResult> Order()
        {
            var userId = (string)null;
            string? role;
            if (User.IsInRole("Admin"))
            {
                role = "Admin";
            }
            else
            {
                role = "customer";
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return View(OrderService.OrderList(userId, role));
        }
        public FileResult pdf(string Ids)
        {
            //var data = Ids.FirstOrDefault();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var templetedata = OrderService.GetTempleteData(Ids, userId);
            var mail = new HtmlTemplate<TemplateViewModel>("Sample", templetedata);
            var htmlstr = mail.GenerateBody();
            StringBuilder sb = new StringBuilder();
            sb.Append(htmlstr);
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString())))
            {
                ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
                PdfWriter writer = new PdfWriter(byteArrayOutputStream);
                PdfDocument pdfDocument = new PdfDocument(writer);
                pdfDocument.SetDefaultPageSize(PageSize.A4);
                HtmlConverter.ConvertToPdf(stream, pdfDocument);
                pdfDocument.Close();
                return File(byteArrayOutputStream.ToArray(), "application/pdf", templetedata.Order.OrderId +".pdf");
            }
        }
       
        private StringBuilder pdf1(string orderId)
        {
            var id = new Guid(orderId);
            List<OrderDetails> OrderDetails = (from det in dbContext.OrderDetails
                                               join prd in dbContext.Products
                                               on det.ProductId equals prd.ProductId
                                               where det.OrderId == id
                                               select new OrderDetails
                                               {
                                                   OrderId = det.OrderId,
                                                   Products = prd,
                                                   UnitPrice = det.UnitPrice,
                                                   Quantity = det.Quantity,
                                                   Discount = det.Discount
                                               }).ToList<OrderDetails>();
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<h4>invoice</h4>");
             sb.Append("<table class='table table - bordered'>");
            sb.Append("<tr>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc;width: 10px'>Id</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc;width: 10px'>Product</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc;width: 10px'>Price</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc;width: 10px'>Quantity</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc;width: 10px'>Discount</th>");
            sb.Append("</tr>");
            for (int i = 0; i < OrderDetails.Count; i++)
            {
                var data = new
                {
                    Id = i,
                    OrderId = OrderDetails[i].OrderId,
                    Products = OrderDetails[i].Products.ProductName,
                    UnitPrice = OrderDetails[i].UnitPrice,
                    Quantity = (int)OrderDetails[i].Quantity,
                    Discount = OrderDetails[i].Discount
                };
                sb.Append("<tr>");
                //Append data.
                sb.Append("<td style='border: 1px solid #ccc;width: 10px'>");
                sb.Append(data.Id);
                sb.Append("</td>");
                sb.Append("<td style='border: 1px solid #ccc;width: 10px'>");
                sb.Append(data.Products);
                sb.Append("</td>");
                sb.Append("<td style='border: 1px solid #ccc;width: 10px'>");
                sb.Append(data.UnitPrice);
                sb.Append("</td>");
                sb.Append("<td style='border: 1px solid #ccc;width: 10px'>");
                sb.Append(data.Quantity);
                sb.Append("</td>");
                sb.Append("<td style='border: 1px solid #ccc;width: 10px'>");
                sb.Append(data.Discount);
                sb.Append("</td>");
                sb.Append("</tr>");
            }
            return sb;
            //Table end.
            //sb.Append("</table>");
            //using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString())))
            //{
            //    ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
            //    PdfWriter writer = new PdfWriter(byteArrayOutputStream);
            //    PdfDocument pdfDocument = new PdfDocument(writer);
            //    pdfDocument.SetDefaultPageSize(PageSize.A4);
            //    HtmlConverter.ConvertToPdf(stream, pdfDocument);
            //    pdfDocument.Close();
            //    return File(byteArrayOutputStream.ToArray(), "application/pdf", "Grid.pdf");
            //}
        }
    }
}

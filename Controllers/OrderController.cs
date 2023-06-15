using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace ECommerceWeb.Controllers
{
    [Authorize]
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
    }
}

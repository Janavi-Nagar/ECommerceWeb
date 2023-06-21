using ECommerceWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;

namespace ECommerceWeb.Controllers
{
    [Route("create-payment-intent")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly UserDbContext dbcontext;
        public PaymentController(UserDbContext _dbcontext)
        {
            dbcontext = _dbcontext;
        }
        [HttpPost]
        public ActionResult Create(PaymentIntentCreateRequest request)
        {
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = (long?)CalculateOrderAmount(request.OrderId),
                Currency = "inr",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });

            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }

        private decimal CalculateOrderAmount(string orderid)
        {
            var orderId = new Guid(orderid);
            var grossamount = dbcontext.Order.FirstOrDefault(m => m.OrderId == orderId).GrossAmount;
            return grossamount;
        }

        public class PaymentIntentCreateRequest
        {
            [JsonProperty("items")]
            public string OrderId { get; set; }
        }
    }
}

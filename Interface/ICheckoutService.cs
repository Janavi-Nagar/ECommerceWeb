using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface ICheckoutService
    {
        Task<Guid> PlaceOrder(CheckoutViewModel model);
        void UpdatePayment(string pi_Id, string status, string orderid);

    }
}

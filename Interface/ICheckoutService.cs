using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface ICheckoutService
    {
        Task<Guid> AddUpdateAddress(CheckoutViewModel model);

    }
}

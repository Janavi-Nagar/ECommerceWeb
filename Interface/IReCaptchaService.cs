using ECommerceWeb.Models.ViewModels;

namespace ECommerceWeb.Interface
{
    public interface IReCaptchaService
    {
        Task<ReCaptchaResponse> Varification(string token);
    }
}

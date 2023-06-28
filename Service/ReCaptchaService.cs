using ECommerceWeb.Interface;
using ECommerceWeb.Models.ViewModels;
using Newtonsoft.Json;
using System.Net;

namespace ECommerceWeb.Service
{
    public class ReCaptchaService : IReCaptchaService
    {
        private readonly IConfiguration config;
        public ReCaptchaService(IConfiguration _config)
        {
            config = _config;
        }
        public async Task<ReCaptchaResponse> Varification(string token)
        {
            string secret = config.GetValue<string>("RecaptchaSettings:RecaptchaSecretKey"); ;
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, token));
            return JsonConvert.DeserializeObject<ReCaptchaResponse>(jsonResult.ToString());
        }
    }
}

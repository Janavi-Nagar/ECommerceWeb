using Newtonsoft.Json;

namespace ECommerceWeb.Models.ViewModels
{
    public class ReCaptchaResponse
    {

        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("score")]
        public float Score { get; set; }
        [JsonProperty("error_codes")]
        public string[] ErrorCodes { get; set; }
    }
}


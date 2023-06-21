using ECommerceWeb.Areas.Identity.Data;
using ECommerceWeb.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace ECommerceWeb.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;

        public UserController(IUserService _userService, UserManager<ApplicationUser> userManager, IOrderService orderService)
        {
            userService = _userService;
            _userManager = userManager;
            _orderService = orderService;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public async Task<IActionResult> RenderPartial(string inputParam)
        {
            var user = await _userManager.GetUserAsync(User);
            switch (inputParam)
            {
                case "_UserInfo":
                    return PartialView("_UserInfo", user);
                case "_Addresses":
                    var add = userService.GetAddById(user.Id);
                    return PartialView("_Addresses", add);
                case "_Orders":
                    var order = _orderService.OrderList(user.Id, "Customer");
                    return PartialView("_Orders", order);
                case "ChangePass":
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);
                    return Redirect(callbackUrl);
            }
            return null;
        }

    }
}

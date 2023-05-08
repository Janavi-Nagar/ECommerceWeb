using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWeb.Controllers
{
    [Authorize(Policy = "rolecreation")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IActionResult RoleList()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }
        
        public IActionResult RoleForm()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
       
        public async Task<IActionResult> RoleForm(IdentityRole role)
        {
            await roleManager.CreateAsync(role);
            return RedirectToAction("RoleList");
        }
    }
}

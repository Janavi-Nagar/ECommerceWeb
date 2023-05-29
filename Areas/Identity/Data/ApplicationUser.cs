using ECommerceWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerceWeb.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ECommerceWebUser class
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ICollection<Products> Product { get; set; }
}


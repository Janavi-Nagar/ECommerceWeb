using ECommerceWeb.Areas.Identity.Data;
using ECommerceWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWeb.Data;

public class UserDbContext : IdentityDbContext//<ApplicationUser>
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        //builder.Entity<ApplicationUser>();

        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Product)
            .WithOne()
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.ClientNoAction);
    }
    public DbSet<Products> Products { get; set; }
    public DbSet<ProductCategory> ProductCategory { get; set; }
    public DbSet<Cart> Cart { get; set; }
    public DbSet<DiscountCoupon> DiscountCoupon { get; set; }
    public DbSet<CouponProduct> CouponProduct { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<CustomerBilling> CustomerBilling { get; set; }
}

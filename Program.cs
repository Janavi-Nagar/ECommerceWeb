using ECommerceWeb.Areas.Identity.Data;
using ECommerceWeb.Areas.Identity.Pages.Account;
using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;
using CouponService = ECommerceWeb.Service.CouponService;
using ProductService = ECommerceWeb.Service.ProductService;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UserDbContextConnection") ?? throw new InvalidOperationException("Connection string 'UserDbContextConnection' not found.");
StripeConfiguration.ApiKey = builder.Configuration["AppSettings:StripeKeys:SecreteKey"];
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

    //.AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("rolecreation", policy =>
    policy.RequireRole("Admin")
    );
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/MyHttpStatuses/AccessDenied";
    options.LoginPath = "/Identity/Account/Login";
});
builder.Services.AddMvc();
builder.Services.AddSession();
builder.Services.AddTransient<IEmailSender, SendEmail>();
builder.Services.AddTransient<IHomeService, HomeService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();

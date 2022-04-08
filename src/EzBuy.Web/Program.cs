using CloudinaryDotNet;
using EzBuy.Data;
using EzBuy.Models;
using EzBuy.Services;
using EzBuy.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ChargeService>(new ChargeService());
// Add services to the container.
StripeConfiguration.SetApiKey(builder.Configuration["Stripe:TestSecretKey"]);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EzBuyContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("EzBuy.Data")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<EzBuyContext>();

builder.Services.AddTransient<IProductService, ProductsService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.
    Services.
    AddSingleton
    <IConfiguration>
    (builder.Configuration);
builder.Services.AddTransient<ICloudinaryService, CloudinaryService>();
CloudinaryDotNet.Account account = new CloudinaryDotNet.Account(
                builder.Configuration.GetSection("Cloudinary:cloud").Value,
                builder.Configuration.GetSection("Cloudinary:apiKey").Value,
                builder.Configuration.GetSection("Cloudinary:apiSecret").Value);

Cloudinary cloudinary = new Cloudinary(account);
builder.Services.AddSingleton(cloudinary);
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".EzBuy.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

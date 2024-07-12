using E_Commerse_Website.Data;
using E_Commerse_Website.Services.EmailServices;
using E_Commerse_Website.Services.Implimentation;
using E_Commerse_Website.Services.IRepo;
using E_Commerse_Website.Services.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddDbContext<myContext>(o=>o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "CustomerCookie"; // or "AdminCookie" based on your requirement
})
    .AddCookie("AdminCookie", options =>
    {
        options.LoginPath = "/admin/login";
        options.Cookie.Name = "AdminCookie";
        options.ExpireTimeSpan = TimeSpan.FromHours(5);
    })
    .AddCookie("CustomerCookie", options =>
    {
        options.LoginPath = "/customer/login";
        options.Cookie.Name = "CustomerCookie";
        options.ExpireTimeSpan = TimeSpan.FromHours(5);
    });



builder.Services.AddSingleton<EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=customer}/{action=Index}/{id?}");

app.Run();

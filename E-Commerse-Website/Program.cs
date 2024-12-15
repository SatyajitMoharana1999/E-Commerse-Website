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
//builder.Services.AddDbContext<myContext>(o=>o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<myContext>(o=>o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ICartRepo, CartRepo>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "CustomerCookie"; // or "AdminCookie" based on your requirement
})
    .AddCookie("AdminCookie", options =>
    {
        options.LoginPath = "/admin/login";
        options.Cookie.Name = "AdminCookie";
        options.ExpireTimeSpan = TimeSpan.FromHours(5); // Adjust as necessary
        options.Cookie.HttpOnly = true; // Ensures the cookie is only accessible via HTTP(S)
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Use Always for HTTPS, None for HTTP
        options.Cookie.SameSite = SameSiteMode.Strict; // Adjust based on your cross-site requirements
        options.Cookie.IsEssential = true; // Ensures the cookie is always stored
        options.SlidingExpiration = true; // Resets the expiration time on each request
        options.Events = new CookieAuthenticationEvents
        {
            OnSigningIn = async context =>
            {
                context.CookieOptions.Expires = DateTimeOffset.UtcNow.AddHours(5);
            }
        };
    })
    .AddCookie("CustomerCookie", options =>
    {
        options.LoginPath = "/customer/login";
        options.Cookie.Name = "CustomerCookie";
        options.ExpireTimeSpan = TimeSpan.FromHours(5); // Adjust as necessary
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.IsEssential = true;
        options.SlidingExpiration = true;
        options.Events = new CookieAuthenticationEvents
        {
            OnSigningIn = async context =>
            {
                context.CookieOptions.Expires = DateTimeOffset.UtcNow.AddHours(5);
            }
        };
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

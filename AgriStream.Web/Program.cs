using AgriStream.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Azure SQL Database Context
builder.Services.AddDbContext<AgriStreamContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Cookie Authentication Services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

// Register Blob Storage Service for Task C
builder.Services.AddTransient<BlobStorageService>();

// Register AgriStream API Service for Azure Functions communication
builder.Services.AddHttpClient<AgriStreamApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// MUST be placed between UseRouting and UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// Updated default route pattern to launch directly to Account/Signup
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Signup}/{id?}");

app.Run();
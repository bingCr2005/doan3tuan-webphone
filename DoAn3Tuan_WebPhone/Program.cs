using DoAn3Tuan_WebPhone.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// ??ng ký DbContext và chu?i k?t n?i
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBBanDienThoai")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DBBanDienThoaiContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

// Route cho Trang cá nhân (ví d?: domain.com/ca-nhan)
app.MapControllerRoute(
    name: "trang-ca-nhan",
    pattern: "ca-nhan",
    defaults: new { controller = "Profile", action = "Index" });

// Route cho Gi? hàng (ví d?: domain.com/gio-hang)
app.MapControllerRoute(
    name: "gio-hang",
    pattern: "gio-hang",
    defaults: new { controller = "Cart", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

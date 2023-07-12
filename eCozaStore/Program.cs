
using eCozaStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Kết nối csdl
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<dbCozaStoreContext>(options => options.UseSqlServer(connection));


builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[]
{
    UnicodeRanges.All
}));

//Session
builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession();
builder.Services.AddSession(cfg =>
{            // Đăng ký dịch vụ Session
    cfg.Cookie.Name = "eCozaStore";             // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
    cfg.IdleTimeout = new TimeSpan(0, 30, 0);   // Thời gian tồn tại của Session
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(p =>
               {
                   p.Cookie.Name = "UserLoginCookie";
                   p.ExpireTimeSpan = TimeSpan.FromDays(1);
                   p.LoginPath = "/login.html";
                   p.LoginPath = "/adminlogin.html";
                   //p.LogoutPath = "/logout.html";
                   p.AccessDeniedPath = "/not-found.html";
               }
);


var app = builder.Build();

// Test upload hình ảnh
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
              Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
        ),

    RequestPath = "/files"
});


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

//cart
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// Cấu hình khi tạo Area Admin

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

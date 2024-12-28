var builder = WebApplication.CreateBuilder(args);

// Khai báo để hệ thống sử dụng mô hình MVC
builder.Services.AddControllersWithViews();

// Khai báo session
builder.Services.AddSession();

// Đăng ký IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Đọc cấu hình từ appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Đăng ký EmailService như một dịch vụ
builder.Services.AddTransient<EmailService>();

var app = builder.Build();

// Sử dụng session
app.UseSession();

// Khai báo để project hiểu được và có thể load các file trong thư mục wwwroot
app.UseStaticFiles();

// Cấu hình đường dẫn
app.MapControllerRoute(
    name: "area",
    pattern: "{area}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

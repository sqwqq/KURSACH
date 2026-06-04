using Microsoft.EntityFrameworkCore;
using Kursach.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var dbPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "CollegeSports.db");
Directory.CreateDirectory(Path.GetDirectoryName(dbPath) ?? "App_Data");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// 1. Регистрируем сервис сессий
builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
    SeedData.Initialize(context);
}

app.UseStaticFiles();
app.UseRouting();

app.UseExceptionHandler("/error");
app.UseStatusCodePagesWithReExecute("/error/{0}");

// 2. Включаем middleware сессий
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
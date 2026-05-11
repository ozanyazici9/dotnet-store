using dotnet_store.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString);
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
app.UseAuthorization();

// runtime da upload edilen statik dosyaları serve etmez. Çünkü compile time da mevcut olan statik dosyaları sıkıştırır ve performas sağlayarak daha hızlı serve edebilir. Runtime da gelen dosya compile time da bulunmadığı için onu serve edemez.
//app.MapStaticAssets();

// runtime da upload edilen statik dosyaları serve edebilir yani bir url ile ulaşmanı sağlar.
app.UseStaticFiles();

// urunler/telefon

app.MapControllerRoute(
        name: "urunler_by_kategori",
        pattern: "urunler/{url?}",
        defaults: new { controller = "Urun", action = "List" }
    )
    .WithStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

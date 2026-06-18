using Microsoft.AspNetCore.Identity;

namespace dotnet_store.Models;

public static class SeedDatabase
{
    public static async void Initialize(IApplicationBuilder app)
    {
        var userManager = app
            .ApplicationServices.CreateScope()
            .ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        var roleManager = app
            .ApplicationServices.CreateScope()
            .ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        // Uygulama yayınlandığında eğer bir admin yok ise db de giriş yapamayız bunu elle db ye eklemek yerine bu durumu kontrol edip otomatik bir admin kullanıcısı ekliyoruz ki giriş yapabilelim.
        if (!roleManager.Roles.Any())
        {
            var admin = new AppRole { Name = "Admin" };
            await roleManager.CreateAsync(admin);

            var customer = new AppRole { Name = "Customer" };
            await roleManager.CreateAsync(customer);
        }

        if (!userManager.Users.Any())
        {
            var admin = new AppUser
            {
                AdSoyad = "Ozan Yazıcı",
                Email = "ozanyazici9@gmail.com",
                UserName = "ozanyazici",
            };

            await userManager.CreateAsync(admin, "1234567");
            await userManager.AddToRoleAsync(admin, "Admin");

            var customer = new AppUser
            {
                AdSoyad = "Sare Yazıcı",
                Email = "sareyazici9@gmail.com",
                UserName = "sareyazici",
            };

            await userManager.CreateAsync(customer, "1234567");
            await userManager.AddToRoleAsync(customer, "Customer");
        }
    }
}

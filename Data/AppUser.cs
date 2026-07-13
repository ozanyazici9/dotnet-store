using Microsoft.AspNetCore.Identity;

namespace dotnet_store.Data;

public class AppUser : IdentityUser<int>
{
    public string AdSoyad { get; set; } = null!;
}

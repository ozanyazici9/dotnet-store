namespace dotnet_store.Models;

public class UserGetModel
{
    public int Id { get; set; }
    public string AdSoyad { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string> UserRole { get; set; } = null!;
}
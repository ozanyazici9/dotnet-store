using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class UserEditModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Ad Soyad")]
    public string AdSoyad { get; set; } = null!;

    [Required]
    [Display(Name = "Eposta")]
    [EmailAddress]
    public string Email { get; set; } = null!;
    public List<string> SelectedRoles { get; set; } = [];
    public List<string> UserRoles { get; set; } = [];
    public List<string> AllRoles { get; set; } = [];

    [Display(Name = "Parola")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Display(Name = "Parola Tekrar")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Parola eşleşmiyor")]
    public string? PasswordConfirm { get; set; }
}

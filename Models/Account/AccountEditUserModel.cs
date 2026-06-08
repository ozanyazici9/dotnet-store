using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class AccountEditUserModel
{
    [Required]
    [StringLength(50)]
    [Display(Name = "Ad Soyad")]
    // [RegularExpression("^[a-zA-z0-9]*$", ErrorMessage = "Sadece sayı ve harf giriniz")]
    public string AdSoyad { get; set; } = null!;

    [Required]
    [Display(Name = "Eposta")]
    [EmailAddress]
    public string Email { get; set; } = null!;
}

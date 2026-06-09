using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class AccountChangePasswordModel
{
    [Required]
    [Display(Name = "Mevcut Parola")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = null!;

    [Required]
    [Display(Name = "Yeni Parola")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = null!;

    [Required]
    [Display(Name = "Yeni Parola Tekrar")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Parola eşleşmiyor")]
    public string ConfirmNewPassword { get; set; } = null!;
}

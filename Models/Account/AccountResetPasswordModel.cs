using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class AccountResetPasswordModel
{
    public string Token { get; set; } = null!;

    public string Email { get; set; } = null!;

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

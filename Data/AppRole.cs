using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace dotnet_store.Models;

public class AppRole : IdentityRole<int>
{
    // [Required]
    // [Display(Name = "Role Adı")]
    // [StringLength(30)]
    // public string RoleAdi { get; set; } = null!;
}

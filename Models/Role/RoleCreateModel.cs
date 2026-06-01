using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class RoleCreateModel
{
    [Required]
    [StringLength(30)]
    [Display(Name = "Kategori Adı")]
    public string RoleAdi { get; set; } = null!;
}
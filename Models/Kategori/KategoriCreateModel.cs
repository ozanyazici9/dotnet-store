using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class KategoriCreateModel
{
    [Required]
    [StringLength(30)]
    [Display(Name = "Kategori Adı")]
    public string KategoriAdi { get; set; } = null!;

    [Required]
    [StringLength(30)]
    [Display(Name = "URL")]
    public string Url { get; set; } = null!;
}

// Buradaki display annotaion ları formlarda name id ve value değerlerini değiştirir. Formda bunları yazmaya gerek yok. asp-for ile değerleri alır.

// KategoriCreateModel in amacı Formu post ettiğimizde Controllerda tek tek değişken yazmak yerine bu modeli kullanmak.

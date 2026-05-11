using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class SliderCreateModel
{
    [Display(Name = "Başlık")]
    public string? Baslik { get; set; }

    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }

    [Display(Name = "Slider Resim")]
    public IFormFile Resim { get; set; } = null!;

    [Display(Name = "Sıra")]
    public int Index { get; set; }

    [Display(Name = "Aktif mi?")]
    public bool Aktif { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class SliderEditModel
{
    public int Id { get; set; }

    [Display(Name = "Başlık")]
    public string? Baslik { get; set; }

    [Display(Name = "Açıklama")]
    public string? Aciklama { get; set; }

    [Display(Name = "Slider Resim")]
    public string ResimAdi { get; set; } = null!;
    public IFormFile? ResimDosyasi { get; set; }

    [Display(Name = "Sıra")]
    public int Index { get; set; }

    [Display(Name = "Aktif mi?")]
    public bool Aktif { get; set; }
}

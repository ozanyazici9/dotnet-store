using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class OrderCreateModel
{
    [Required(ErrorMessage = "Ad Soyad alanı bos gecilemez")]
    [StringLength(50)]
    [Display(Name = "Ad Soyad")]
    public string AdSoyad { get; set; } = null!;

    [Required(ErrorMessage = "Sehir alanı bos gecilemez")]
    [StringLength(50)]
    [Display(Name = "Şehir")]
    public string Sehir { get; set; } = null!;

    [Required(ErrorMessage = "Adres Satırı alanı bos gecilemez")]
    [StringLength(200)]
    [Display(Name = "Adres Satırı")]
    public string AdresSatiri { get; set; } = null!;

    [Required(ErrorMessage = "Posta Kodu alanı bos gecilemez")]
    [StringLength(5)]
    [Display(Name = "Posta Kodu")]
    public string PostaKodu { get; set; } = null!;

    [Required(ErrorMessage = "Telefon Numarası alanı bos gecilemez")]
    [StringLength(11)]
    [Display(Name = "Telefon Numarası")]
    [Phone]
    public string Telefon { get; set; } = null!;

    [Display(Name = "Sipariş Notu")]
    [StringLength(200)]
    public string? SiparisNotu { get; set; } = null!;
}

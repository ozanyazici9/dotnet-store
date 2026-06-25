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

    [Required(ErrorMessage = "Kart İsmi alanı bos gecilemez")]
    [StringLength(50)]
    [Display(Name = "Kart üzerindeki isim")]
    public string CartName { get; set; } = null!;

    [Required(ErrorMessage = "Kart Numarası alanı bos gecilemez")]
    [StringLength(16)]
    [Display(Name = "Kart Numarası")]
    public string CartNumber { get; set; } = null!;

    [Required(ErrorMessage = "Kart geçerlilik yılı alanı bos gecilemez")]
    [StringLength(4)]
    [Display(Name = "Kart Geçerlilik Yılı")]
    public string CartExpirationYear { get; set; } = null!;

    [Required(ErrorMessage = "Kart geçerlilik ayı alanı bos gecilemez")]
    [StringLength(2)]
    [Display(Name = "Kart Geçerlilik Ayı")]
    public string CartExpirationMonth { get; set; } = null!;

    [Required(ErrorMessage = "Kart CVV alanı bos gecilemez")]
    [StringLength(3)]
    [Display(Name = "Kart cvv")]
    public string CartCVV { get; set; } = null!;
}

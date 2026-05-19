using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class UrunModel
{
    [Display(Name = "Ürün Adı")]
    [Required(ErrorMessage = "{0} zorunludur")]
    [StringLength(
        50,
        ErrorMessage = "{0} {2}-{1} karakter sayısı arasında olmalıdır",
        MinimumLength = 10
    )]
    public string UrunAdi { get; set; } = null!;

    [Display(Name = "Ürün Fiyat")]
    [Range(
        0,
        10000000.00,
        ErrorMessage = "{0} için girdiğiniz değer {1} ile {2} arasında olmalıdır."
    )]
    [DataType(DataType.Currency)]
    [Required(ErrorMessage = "{0} zorunludur")]
    public double? Fiyat { get; set; }

    [Display(Name = "Ürün Resmi")]
    [DataType(DataType.Upload)]
    public IFormFile? Resim { get; set; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Ürün Açıklama")]
    public string? Aciklama { get; set; }
    public bool Aktif { get; set; }
    public bool Anasayfa { get; set; }

    [Display(Name = "Kategori")]
    [Required(ErrorMessage = "{0} zorunludur")]
    public int? KategoriId { get; set; }
}

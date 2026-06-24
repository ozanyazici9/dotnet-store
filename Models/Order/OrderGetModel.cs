namespace dotnet_store.Models;

public class OrderGetModel
{
    public int Id { get; set; }
    public DateTime SiparisTarihi { get; set; }
    public string AdSoyad { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Sehir { get; set; } = null!;
    public string AdresSatiri { get; set; } = null!;
    public string PostaKodu { get; set; } = null!;
    public string Telefon { get; set; } = null!;
    public double ToplamFiyat { get; set; }
    public string? SiparisNotu { get; set; }
    public List<OrderItemModel> OrderItems { get; set; } = new();
    public double AraToplam { get; set; }
    public double Vergi { get; set; }
}

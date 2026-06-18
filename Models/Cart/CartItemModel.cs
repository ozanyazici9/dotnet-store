namespace dotnet_store.Models;

public class CartItemModel
{
    public int CartItemId { get; set; }
    public string CustomerId { get; set; } = null!;
    public string UrunAdi { get; set; } = null!;
    public int UrunId { get; set; }
    public double Fiyat { get; set; }
    public string? Resim { get; set; }
    public int Miktar { get; set; }
}

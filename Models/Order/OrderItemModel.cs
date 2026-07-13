namespace dotnet_store.Models;

public class OrderItemModel
{
    public int Id { get; set; }
    public int UrunId { get; set; }
    public UrunGetModel Urun { get; set; } = null!;
    public double Fiyat { get; set; }
    public int Miktar { get; set; }
}

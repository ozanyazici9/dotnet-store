namespace dotnet_store.Models;

public class CartViewModel
{
    public List<CartItemModel> CartItems { get; set; } = new();
    public double AraToplam { get; set; }
    public double Toplam { get; set; }
}

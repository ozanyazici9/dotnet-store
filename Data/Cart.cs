namespace dotnet_store.Models;

public class Cart
{
    public int CartId { get; set; }

    // CustomerId property Username bilgisini taşır.
    public string CustomerId { get; set; } = null!;

    // CarItem a ulaşmak için (Include)
    public List<CartItem> CartItems { get; set; } = new();

    public void AddItem(int urunId, int miktar)
    {
        var item = CartItems.FirstOrDefault(i => i.UrunId == urunId);

        if (item != null)
        {
            item.Miktar += miktar;
        }
        else
        {
            CartItems.Add(new CartItem { UrunId = urunId, Miktar = miktar });
        }
    }

    public void DeleteItem(int urunId, int miktar)
    {
        var item = CartItems.FirstOrDefault(i => i.UrunId == urunId);

        if (item != null)
        {
            item.Miktar -= miktar;

            if (item.Miktar == 0)
            {
                CartItems.Remove(item);
            }
        }
    }

    public double AraToplam()
    {
        return CartItems.Sum(i => i.Urun.Fiyat * i.Miktar);
    }

    public double Toplam()
    {
        return CartItems.Sum(i => i.Urun.Fiyat * i.Miktar) * 1.2;
    }
}

public class CartItem
{
    public int CartItemId { get; set; }

    public int UrunId { get; set; }

    // Urun bilgilerine ulaşmak için
    public Urun Urun { get; set; } = null!;

    public int CartId { get; set; }

    // Cart bilgilerine ulaşmak için
    public Cart Cart { get; set; } = null!;

    public int Miktar { get; set; }
}

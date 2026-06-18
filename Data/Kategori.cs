namespace dotnet_store.Models;

// entity

public class Kategori
{
    public int Id { get; set; }
    public string KategoriAdi { get; set; } = null!;
    public string Url { get; set; } = null!; // Seo için uzantıda görünecek isim
    public List<Urun> Uruns { get; set; } = new(); // bir kategorinin birden fazla ürünü var
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Data;

public class DataContext : IdentityDbContext<AppUser, AppRole, int>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Urun> Urunler { get; set; }
    public DbSet<Kategori> Kategoriler { get; set; }
    public DbSet<Slider> SliderImages { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Kategori>()
            .HasData(
                new List<Kategori>()
                {
                    new Kategori
                    {
                        Id = 1,
                        KategoriAdi = "Telefon",
                        Url = "telefon",
                    },
                    new Kategori
                    {
                        Id = 2,
                        KategoriAdi = "Beyaz Eşya",
                        Url = "beyaz-esya",
                    },
                    new Kategori
                    {
                        Id = 3,
                        KategoriAdi = "Giyim",
                        Url = "giyim",
                    },
                    new Kategori
                    {
                        Id = 4,
                        KategoriAdi = "Elektronik",
                        Url = "elektronik",
                    },
                    new Kategori
                    {
                        Id = 5,
                        KategoriAdi = "Kozmetik",
                        Url = "kozmetik",
                    },
                    new Kategori
                    {
                        Id = 6,
                        KategoriAdi = "Kitap",
                        Url = "kitap",
                    },
                    new Kategori
                    {
                        Id = 7,
                        KategoriAdi = "Kategori 1",
                        Url = "kategori-1",
                    },
                    new Kategori
                    {
                        Id = 8,
                        KategoriAdi = "Kategori 2",
                        Url = "kategori-2",
                    },
                    new Kategori
                    {
                        Id = 9,
                        KategoriAdi = "Kategori 3",
                        Url = "kategori-3",
                    },
                    new Kategori
                    {
                        Id = 10,
                        KategoriAdi = "Kategori 4",
                        Url = "kategori-4",
                    },
                }
            );

        modelBuilder
            .Entity<Urun>()
            .HasData(
                new List<Urun>()
                {
                    new Urun
                    {
                        Id = 1,
                        UrunAdi = "Samsung Galaxy S20",
                        Fiyat = 40000,
                        Aktif = true,
                        Resim = "1.jpeg",
                        Anasayfa = true,
                        Aciklama =
                            "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.",
                        KategoriId = 1,
                    },
                    new Urun
                    {
                        Id = 2,
                        UrunAdi = "Samsung Galaxy S21",
                        Fiyat = 50000,
                        Aktif = true,
                        Resim = "2.jpeg",
                        Anasayfa = true,
                        Aciklama =
                            "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.",
                        KategoriId = 2,
                    },
                    new Urun
                    {
                        Id = 3,
                        UrunAdi = "Samsung Galaxy S22",
                        Fiyat = 60000,
                        Aktif = true,
                        Resim = "3.jpeg",
                        Anasayfa = true,
                        Aciklama =
                            "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.",
                        KategoriId = 2,
                    },
                    new Urun
                    {
                        Id = 4,
                        UrunAdi = "Samsung Galaxy S23",
                        Fiyat = 70000,
                        Aktif = true,
                        Resim = "4.jpeg",
                        Anasayfa = false,
                        Aciklama =
                            "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.",
                        KategoriId = 3,
                    },
                    new Urun
                    {
                        Id = 5,
                        UrunAdi = "Samsung Galaxy S24",
                        Fiyat = 80000,
                        Aktif = true,
                        Resim = "5.jpeg",
                        Anasayfa = false,
                        Aciklama =
                            "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.",
                        KategoriId = 3,
                    },
                    new Urun
                    {
                        Id = 6,
                        UrunAdi = "Samsung Galaxy S25",
                        Fiyat = 90000,
                        Aktif = false,
                        Resim = "6.jpeg",
                        Anasayfa = true,
                        Aciklama =
                            "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.",
                        KategoriId = 4,
                    },
                    new Urun
                    {
                        Id = 7,
                        UrunAdi = "Samsung Galaxy S26",
                        Fiyat = 100000,
                        Aktif = true,
                        Resim = "7.jpeg",
                        Anasayfa = false,
                        Aciklama =
                            "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.",
                        KategoriId = 4,
                    },
                    new Urun
                    {
                        Id = 8,
                        UrunAdi = "Samsung Galaxy S27",
                        Fiyat = 110000,
                        Aktif = false,
                        Resim = "8.jpeg",
                        Anasayfa = true,
                        Aciklama =
                            "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.",
                        KategoriId = 1,
                    },
                    new Urun
                    {
                        Id = 9,
                        UrunAdi = "Beyninizin Duygusal hayatı",
                        Fiyat = 1000,
                        Aktif = true,
                        Resim = "9.jpeg",
                        Anasayfa = true,
                        Aciklama =
                            "Lorem ipsum dolor sit amet consectetur, adipisicing elit. Eligendi cumque iste magni, deleniti ipsa esse quae nesciunt quod autem maxime minus facere mollitia ullam corporis similique magnam enim dolorem alias.",
                        KategoriId = 6,
                    },
                }
            );

        modelBuilder
            .Entity<Slider>()
            .HasData(
                new List<Slider>
                {
                    new Slider
                    {
                        Id = 1,
                        ImageUrl = "slider-1.jpeg",
                        Aciklama = "slider1 aciklama",
                        Baslik = "slider1",
                        Aktif = true,
                        Index = 0,
                    },
                    new Slider
                    {
                        Id = 2,
                        ImageUrl = "slider-2.jpeg",
                        Aciklama = "slider2 aciklama",
                        Aktif = true,
                        Index = 1,
                        Baslik = "slider2",
                    },
                    new Slider
                    {
                        Id = 3,
                        ImageUrl = "slider-3.jpeg",
                        Aciklama = "slider3 aciklama",
                        Aktif = true,
                        Index = 2,
                        Baslik = "slider3",
                    },
                }
            );
    }
}

//Connection String
//Migrations

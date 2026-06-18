using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Controllers;

public class CartController : Controller
{
    private readonly DataContext _context;
    private readonly UserManager<AppUser> _userManager;

    public CartController(DataContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<ActionResult> Index()
    {
        var cart = await GetCart();

        var model = new CartViewModel
        {
            CartItems = cart
                .CartItems.Select(i => new CartItemModel
                {
                    CartItemId = i.CartItemId,
                    CustomerId = cart.CustomerId,
                    UrunAdi = i.Urun.UrunAdi,
                    UrunId = i.UrunId,
                    Fiyat = i.Urun.Fiyat,
                    Miktar = i.Miktar,
                    Resim = i.Urun.Resim,
                })
                .ToList(),
            AraToplam = cart.AraToplam(),
            Toplam = cart.Toplam(),
        };

        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> AddToCart(int urunId, int miktar = 1)
    {
        var cart = await GetCart();

        var item = cart.CartItems.FirstOrDefault(i => i.UrunId == urunId);

        if (item != null)
        {
            // daha önce aynı ürün eklenmiş
            item.Miktar++;
        }
        else
        {
            // ilk defa ekleniyor
            cart.CartItems.Add(new CartItem { UrunId = urunId, Miktar = miktar });
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    private async Task<Cart> GetCart()
    {
        // kulannıcı giriş yapmamışsa customerıd yi cookie den çek
        var customerId = User.Identity?.Name ?? Request.Cookies["customerId"];

        var cart = await _context
            .Carts.Where(i => i.CustomerId == customerId) // sadece bu kullanıcının sepeti
            .Include(i => i.CartItems) // CartItem'ları da getir (JOIN)
                .ThenInclude(i => i.Urun) // Her CartItem'ın Urun'unu da getir
            .FirstOrDefaultAsync(); // o kullanıcıya ait tek sepeti al

        if (cart == null)
        {
            // customerId nin olupda cart ın olmadığı durumlar olabilir kullanıcı yeni kayıt olmuştur ve sepeti henüz oluşturulmamıştır veya bir şekilde cookideki cart nesnesi silinmiştir. Bu durumlarada tekrar customerId oluşturmaya gerek yoktur bu yüzden kontrol yapılır.
            customerId = User.Identity?.Name;

            if (string.IsNullOrEmpty(customerId))
            {
                // customerıd yi cookie den de alamazsa customerıd oluşturuyoruz ve bunu cart oluşturmada kullanıyoruz
                customerId = Guid.NewGuid().ToString();

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1),
                    IsEssential = true,
                };

                // cookie sunucu tarafında burada oluşturuluyor ve response da client a variliyor. Bizde GetCart da cookie yi client dan gelen request üzerinden okuyarak kontrol ediyoruz
                Response.Cookies.Append("customerId", customerId, cookieOptions);
            }

            cart = new Cart { CustomerId = customerId };
            _context.Carts.Add(cart); // eğer cart null ise kullanıcı kayıtlı değildir. burada da savechanges demiyoruz ama change tracking sistemi sayesinde context e eklemiş oluyoruz ve cart nesnesi kontrolümüz altında oluyor daha sonra cart ı geri döndürüyoruz. eğer giriş yapamamış kullanıcı sepete ürün eklerse cart a ürünüde ekleyip db ye kaydediyoruz.
        }

        return cart;
    }

    public ActionResult Checkout()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Checkout(int id)
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> RemoveItem(int cartItemId)
    {
        var cart = await GetCart();
        var item = cart.CartItems.FirstOrDefault(i => i.CartItemId == cartItemId);

        if (item != null)
        {
            cart.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            TempData["Mesaj"] = $"{item.Urun.UrunAdi} Ürünü sepetinizden kaldırıldı.";
        }
        else
        {
            TempData["Mesaj"] = "Ürün Bulunamadı";
        }

        return RedirectToAction("Index");
    }
}

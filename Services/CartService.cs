using dotnet_store.Exceptions;
using dotnet_store.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Services;

public interface ICartService
{
    Task<Cart> GetCart(string customerUserName);
    Task AddToCart(int urunId, int miktar, string userName);
    Task RemoveItem(int urunId, int miktar, string userName);
    Task TransferCartToUser(string userName);
}

public class CartService : ICartService
{
    private readonly DataContext _context;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddToCart(int urunId, int miktar, string userName)
    {
        var cart = await GetCart(userName);

        var urun = await _context.Urunler.FirstOrDefaultAsync(i => i.Id == urunId);

        if (urun == null)
            throw new NotFoundException($"Id: {urunId} olan ürün bulunamadı");

        cart.AddItem(urunId, miktar);

        await _context.SaveChangesAsync();
    }

    public async Task<Cart> GetCart(string customerUserName)
    {
        var cart = await _context
            .Carts.Where(i => i.CustomerId == customerUserName) // sadece bu kullanıcının sepeti
            .Include(i => i.CartItems) // CartItem'ları da getir (JOIN)
                .ThenInclude(i => i.Urun) // Her CartItem'ın Urun'unu da getir
            .FirstOrDefaultAsync(); // o kullanıcıya ait tek sepeti al

        if (cart == null)
        {
            // customerId nin olupda cart ın olmadığı durumlar olabilir kullanıcı yeni kayıt olmuştur ve sepeti henüz oluşturulmamıştır veya bir şekilde cookideki cart nesnesi silinmiştir. Bu durumlarada tekrar customerId oluşturmaya gerek yoktur bu yüzden kontrol yapılır.
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                // customerıd yi cookie den de alamazsa customerıd oluşturuyoruz ve bunu cart oluşturmada kullanıyoruz
                userName = Guid.NewGuid().ToString();

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1),
                    IsEssential = true,
                };

                // cookie sunucu tarafında burada oluşturuluyor ve response da client a variliyor. Bizde GetCart da cookie yi client dan gelen request üzerinden okuyarak kontrol ediyoruz
                _httpContextAccessor.HttpContext?.Response.Cookies.Append(
                    "customerId",
                    userName,
                    cookieOptions
                );
            }

            cart = new Cart { CustomerId = userName };
            _context.Carts.Add(cart); // eğer cart null ise kullanıcı kayıtlı değildir. burada da savechanges demiyoruz ama change tracking sistemi sayesinde context e eklemiş oluyoruz ve cart nesnesi kontrolümüz altında oluyor daha sonra cart ı geri döndürüyoruz. eğer giriş yapamamış kullanıcı sepete ürün eklerse cart a ürünüde ekleyip db ye kaydediyoruz.
        }

        return cart;
    }

    public async Task RemoveItem(int urunId, int miktar, string userName)
    {
        var cart = await GetCart(userName);

        var item = _context.Urunler.FirstOrDefault(i => i.Id == urunId);

        if (item == null)
        {
            throw new NotFoundException($"Id: {urunId} olan ürün bulunamadı");
        }

        cart.DeleteItem(urunId, miktar);

        await _context.SaveChangesAsync();
    }

    public async Task TransferCartToUser(string userName)
    {
        var userCart = await GetCart(userName);

        var cookieCart = await GetCart(
            _httpContextAccessor.HttpContext?.Request.Cookies["customerId"]!
        );

        foreach (var item in cookieCart?.CartItems!)
        {
            var cartItem = userCart?.CartItems.FirstOrDefault(i => i.UrunId == item.UrunId);

            if (cartItem != null)
            {
                cartItem.Miktar += item.Miktar;
            }
            else
            {
                userCart?.CartItems.Add(
                    new CartItem { UrunId = item.UrunId, Miktar = item.Miktar }
                );
            }
        }

        _context.Carts.Remove(cookieCart);

        await _context.SaveChangesAsync();
    }
}

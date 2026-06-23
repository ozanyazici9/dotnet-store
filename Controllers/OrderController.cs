using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace dotnet_store.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly ICartService _cartService;
    private readonly DataContext _context;

    public OrderController(ICartService cartService, DataContext context)
    {
        _cartService = cartService;
        _context = context;
    }

    public async Task<ActionResult> Checkout()
    {
        ViewBag.Cart = await _cartService.GetCart(User.Identity?.Name!);

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Checkout(OrderCreateModel model)
    {
        var userName = _cartService.GetCustomerId();

        var cart = await _cartService.GetCart(userName);

        if (cart.CartItems.Count == 0)
        {
            ModelState.AddModelError("", "Sepetinizde ürün yok");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Cart = cart;
            return View(model);
        }

        var order = new Order
        {
            UserName = userName,
            AdSoyad = model.AdSoyad,
            Sehir = model.Sehir,
            AdresSatiri = model.AdresSatiri,
            PostaKodu = model.PostaKodu,
            Telefon = model.Telefon,
            SiparisNotu = model.SiparisNotu!,
            SiparisTarihi = DateTime.Now,
            ToplamFiyat = cart.Toplam(),
            OrderItems = cart
                .CartItems.Select(i => new OrderItem
                {
                    Urun = i.Urun,
                    UrunId = i.UrunId,
                    Miktar = i.Miktar,
                    Fiyat = i.Urun.Fiyat,
                })
                .ToList(),
        };

        await _context.Orders.AddAsync(order);
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();

        return RedirectToAction("Completed", new { orderId = order.Id });
    }

    public async Task<ActionResult> Completed(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order == null)
        {
            //hata fırlat tempdata yaz
            return RedirectToAction("Home");
        }

        return View(orderId);
    }
}

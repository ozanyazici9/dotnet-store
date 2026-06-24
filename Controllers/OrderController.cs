using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public async Task<ActionResult> Index()
    {
        var Orders = await _context
            .Orders.Select(i => new OrderGetModel
            {
                Id = i.Id,
                SiparisTarihi = i.SiparisTarihi,
                AdresSatiri = i.AdresSatiri,
                PostaKodu = i.PostaKodu,
                Sehir = i.Sehir,
                SiparisNotu = i.SiparisNotu,
                AdSoyad = i.AdSoyad,
                Telefon = i.Telefon,
                ToplamFiyat = i.ToplamFiyat,
                UserName = i.UserName,
                OrderItems = i
                    .OrderItems.Select(i => new OrderItemModel
                    {
                        Fiyat = i.Fiyat,
                        Id = i.Id,
                        Miktar = i.Miktar,
                        Urun = i.Urun,
                        UrunId = i.UrunId,
                    })
                    .ToList(),
            })
            .ToListAsync();

        return View(Orders);
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

    public ActionResult Details(int Id)
    {
        var order = _context
            .Orders.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Urun)
            .FirstOrDefault(o => o.Id == Id);

        if (order == null)
            return NotFound();

        var model = new OrderGetModel
        {
            Id = order.Id,
            SiparisTarihi = order.SiparisTarihi,
            AdresSatiri = order.AdresSatiri,
            PostaKodu = order.PostaKodu,
            Sehir = order.Sehir,
            SiparisNotu = order.SiparisNotu,
            AdSoyad = order.AdSoyad,
            Telefon = order.Telefon,
            ToplamFiyat = order.ToplamFiyat,
            UserName = order.UserName,
            OrderItems = order
                .OrderItems.Select(oi => new OrderItemModel
                {
                    Id = oi.Id,
                    UrunId = oi.UrunId,
                    Miktar = oi.Miktar,
                    Fiyat = oi.Fiyat,
                    Urun = oi.Urun,
                })
                .ToList(),
            AraToplam = order.AraToplam(), // Urun artık yüklü, doğru çalışır
            Vergi = order.Vergi(),
        };

        return View(model);
    }

    public ActionResult OrderList()
    {
        var userName = User.Identity?.Name;
        if (userName == null)
            return NotFound();

        var orders = _context
            .Orders.Where(i => i.UserName == userName)
            .Include(i => i.OrderItems)
                .ThenInclude(i => i.Urun)
            .ToList();

        var model = orders
            .Select(i => new OrderGetModel
            {
                Id = i.Id,
                AdSoyad = i.AdSoyad,
                SiparisTarihi = i.SiparisTarihi,
                AdresSatiri = i.AdresSatiri,
                Sehir = i.Sehir,
                PostaKodu = i.PostaKodu,
                ToplamFiyat = i.ToplamFiyat,
                Telefon = i.Telefon,
                OrderItems = i
                    .OrderItems.Select(i => new OrderItemModel
                    {
                        Fiyat = i.Fiyat,
                        Miktar = i.Miktar,
                        Urun = i.Urun,
                        UrunId = i.UrunId,
                    })
                    .ToList(),
                AraToplam = i.AraToplam(),
                Vergi = i.Vergi(),
            })
            .ToList();

        return View(model);
    }
}

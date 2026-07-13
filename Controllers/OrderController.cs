using dotnet_store.Data;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Controllers;

[Authorize]
public class OrderController : BaseController
{
    private readonly ICartService _cartService;
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;
    private readonly IOrderService _orderService;

    public OrderController(
        ICartService cartService,
        DataContext context,
        IConfiguration configuration,
        IOrderService orderService
    )
    {
        _cartService = cartService;
        _context = context;
        _configuration = configuration;
        _orderService = orderService;
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
                        Urun = new UrunGetModel
                        {
                            UrunAdi = i.Urun.UrunAdi,
                            Resim = i.Urun.Resim,
                            Aciklama = i.Urun.Aciklama,
                            Fiyat = i.Urun.Fiyat,
                            KategoriAdi = i.Urun.Kategori.KategoriAdi,
                        },
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
        var userName = GetCustomerUserName();

        var cart = await _cartService.GetCart(userName);

        if (cart.CartItems.Count == 0)
        {
            ModelState.AddModelError("", "Sepetinizde ürün yok");
            return View(model);
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Cart = cart;
            return View(model);
        }

        var result = await _orderService.Checkout(model, userName, cart);

        if (result.Succeeded == false)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            ViewBag.Cart = cart;
            return View(model);
        }

        return RedirectToAction("Completed", new { orderId = result.Data!.Id });
    }

    public async Task<ActionResult> Completed(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order == null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(orderId);
    }

    public async Task<ActionResult> Details(int Id)
    {
        var result = await _orderService.Details(Id);

        if (result.Succeeded == false)
        {
            TempData["Mesaj"] = "Siparişiniz Bulunamadı";
            return RedirectToAction("Index", "Home");
        }

        return View(result.Data);
    }

    public async Task<ActionResult> OrderList()
    {
        var userName = User.Identity?.Name;

        var result = await _orderService.OrderList(userName!);

        if (result.Succeeded == false)
        {
            TempData["Mesaj"] = result.ErrorMessage;
            return RedirectToAction("Index", "Account");
        }

        return View(result.Data);
    }
}

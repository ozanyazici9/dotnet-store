using dotnet_store.Exceptions;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<ActionResult> Index()
    {
        var customerId = _cartService.GetCustomerId();
        var cart = await _cartService.GetCart(customerId);

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
        try
        {
            await _cartService.AddToCart(urunId, miktar);
            TempData["Mesaj"] = "Ürün sepetinize eklendi";
        }
        catch (NotFoundException e)
        {
            TempData["Mesaj"] = e.Message;
        }

        return RedirectToAction("Index");
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
    public async Task<ActionResult> RemoveItem(int urunId, int miktar)
    {
        try
        {
            await _cartService.RemoveItem(urunId, miktar);
            TempData["Mesaj"] = $"Id: {urunId} olan ürün kaldırıldı";
        }
        catch (NotFoundException e)
        {
            TempData["Mesaj"] = e.Message;
        }

        return RedirectToAction("Index");
    }
}

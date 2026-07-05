using dotnet_store.Exceptions;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

public class CartController : BaseController
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<ActionResult> Index()
    {
        var customerUserName = GetCustomerUserName();
        var cart = await _cartService.GetCart(customerUserName);

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
        await _cartService.AddToCart(urunId, miktar, GetCustomerUserName());
        TempData["Mesaj"] = "Ürün sepetinize eklendi";

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<ActionResult> RemoveItem(int urunId, int miktar)
    {
        await _cartService.RemoveItem(urunId, miktar, GetCustomerUserName());
        TempData["Mesaj"] = $"Id: {urunId} olan ürün kaldırıldı";

        return RedirectToAction("Index");
    }
}

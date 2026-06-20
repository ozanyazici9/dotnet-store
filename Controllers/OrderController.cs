using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace dotnet_store.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly ICartService _cartService;

    public OrderController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<ActionResult> Checkout()
    {
        ViewBag.Cart = await _cartService.GetCart(_cartService.GetCustomerId());

        return View();
    }
}

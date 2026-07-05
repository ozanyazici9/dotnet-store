using dotnet_store.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

public class HomeController : Controller
{
    private readonly DataContext _context;

    public HomeController(DataContext context)
    {
        _context = context;
    }

    public ActionResult Index()
    {
        var urunler = _context.Urunler.Where(urun => urun.Aktif && urun.Anasayfa).ToList();
        ViewData["Kategoriler"] = _context.Kategoriler.ToList();
        return View(urunler);
    }

    public ActionResult Error(int code, string? message)
    {
        ViewBag.Code = code;
        ViewBag.Message = message;

        return View();
    }
}

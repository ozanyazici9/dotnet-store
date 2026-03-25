using dotnet_store.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

public class UrunController : Controller
{
    private readonly DataContext _context;

    public UrunController(DataContext context)
    {
        _context = context;
    }

    public ActionResult Index()
    {
        return View();
    }

    public ActionResult List(string url)
    {
        List<Urun> urunler = [];

        if (url == null)
        {
            urunler = _context.Urunler.Where(urun => urun.Aktif).ToList();
        } else
        {
            urunler = _context.Urunler.Where(urun => urun.Aktif && urun.Kategori.Url == url).ToList();
        }
        return View(urunler);
    }

    public ActionResult Details(int id)
    {
        var urun = _context.Urunler.Find(id);
        if (urun == null)
        {
            return RedirectToAction("List");
        }
        
        ViewData["BenzerUrunler"] = _context.Urunler
            .Where(b => b.KategoriId == urun.KategoriId && b.Id != urun.Id && b.Aktif)
            .Take(4).ToList();

        return View(urun);
    }
}
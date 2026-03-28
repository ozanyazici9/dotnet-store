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

    public ActionResult List(string url, string q)
    {
        var query = _context.Urunler.Where(urun => urun.Aktif); // Queryable

        if (!string.IsNullOrEmpty(url))
        {
            query = query.Where(urun => urun.Kategori.Url == url);
        }

        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(urun => urun.UrunAdi.ToLower().Contains(q.ToLower()));

            ViewData["q"] = q;
        }

        return View(query.ToList());
    }

    public ActionResult Details(int id)
    {
        var urun = _context.Urunler.Find(id);
        if (urun == null)
        {
            return RedirectToAction("List");
        }

        ViewData["BenzerUrunler"] = _context
            .Urunler.Where(b => b.KategoriId == urun.KategoriId && b.Id != urun.Id && b.Aktif)
            .Take(4)
            .ToList();

        return View(urun);
    }
}

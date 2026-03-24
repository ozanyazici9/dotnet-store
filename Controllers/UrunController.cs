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

    public ActionResult List()
    {
        List<Urun> urunler = _context.Urunler.Where(urun => urun.Aktif).ToList();
        return View(urunler);
    }

    public ActionResult Details(int id)
    {
        var urun = _context.Urunler.Find(id);
        return View(urun);
    }
}
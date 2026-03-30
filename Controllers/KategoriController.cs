using dotnet_store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Controllers;

public class KategoriController : Controller
{
    private readonly DataContext _context;

    public KategoriController(DataContext context)
    {
        _context = context;
    }

    public ActionResult Index()
    {
        var kategoriler = _context
            .Kategoriler.Select(k => new KategoriGetModel
            {
                Id = k.Id,
                KategoriAdi = k.KategoriAdi,
                UrunSayisi = k.Uruns.Count,
            })
            .ToList();
        return View(kategoriler);
    }
}

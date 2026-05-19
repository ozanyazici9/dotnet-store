using dotnet_store.Models;
using Microsoft.AspNetCore.Mvc;

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
                Url = k.Url,
                UrunSayisi = k.Uruns.Count,
            })
            .ToList();
        return View(kategoriler);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(KategoriCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var entity = new Kategori() { KategoriAdi = model.KategoriAdi, Url = model.Url };

            _context.Kategoriler.Add(entity);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(model);
    }

    public ActionResult Edit(int id)
    {
        var entity = _context
            .Kategoriler.Select(i => new KategoriEditModel
            {
                Id = i.Id,
                KategoriAdi = i.KategoriAdi,
                Url = i.Url,
            })
            .FirstOrDefault(i => i.Id == id);
        return View(entity);
    }

    [HttpPost]
    public ActionResult Edit(int id, KategoriEditModel model)
    {
        if (id != model.Id)
        {
            return RedirectToAction("Index");
        }

        if (ModelState.IsValid)
        {
            var entity = _context.Kategoriler.FirstOrDefault(i => i.Id == model.Id);

            if (entity != null)
            {
                entity.KategoriAdi = model.KategoriAdi;
                entity.Url = model.Url;

                _context.SaveChanges();

                TempData["Mesaj"] = $"{entity.KategoriAdi} kategorisi güncellendi";

                return RedirectToAction("Index");
            }
        }

        return View(model);
    }
}

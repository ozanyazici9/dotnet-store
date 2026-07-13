using dotnet_store.Data;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

[Authorize(Roles = "Admin")]
public class KategoriController : BaseController
{
    private readonly DataContext _context;

    private readonly IKategoriService _kategoriService;

    public KategoriController(DataContext context, IKategoriService kategoriService)
    {
        _context = context;
        _kategoriService = kategoriService;
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
        if (!ModelState.IsValid)
            return View(model);

        _kategoriService.Create(model.KategoriAdi, model.Url);

        TempData["Mesaj"] = $"{model.KategoriAdi} kategorisi eklendi";
        return RedirectToAction("Index");
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
            return RedirectToAction("Index");

        if (!ModelState.IsValid)
            return View(model);

        _kategoriService.Edit(model.Id, model.KategoriAdi, model.Url);

        TempData["Mesaj"] = $"{model.KategoriAdi} kategorisi güncellendi";
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Delete(int? id)
    {
        if (id == null)
            return RedirectToAction("Index");

        // Silme işlemi tek satırda şöyle de yapılabilir; _context.Kategoriler.Where(i => i.Id == id).ExecuteDelete();

        var entity = await _context.Kategoriler.FindAsync(id);

        if (entity == null)
            return RedirectToAction("Index");

        return View(entity);
    }

    [HttpPost]
    public async Task<ActionResult> DeleteConfirm(int? id)
    {
        if (id == null)
            return RedirectToAction("Index");

        // Silme işlemi tek satırda şöyle de yapılabilir; _context.Kategoriler.Where(i => i.Id == id).ExecuteDelete();

        await _kategoriService.Delete(id.Value);

        TempData["Mesaj"] = "Kategori silindi";
        return RedirectToAction("Index");
    }
}

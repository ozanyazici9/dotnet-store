using dotnet_store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dotnet_store.Controllers;

public class UrunController : Controller
{
    private readonly DataContext _context;

    public UrunController(DataContext context)
    {
        _context = context;
    }

    public ActionResult Index(int? kategori)
    {
        var query = _context.Urunler.AsQueryable();

        if (kategori != null)
        {
            query = query.Where(i => i.KategoriId == kategori);
        }

        // Include olmadan da çalışır
        var urunler = query
            .Select(u => new UrunGetModel
            {
                Id = u.Id,
                Resim = u.Resim,
                Fiyat = u.Fiyat,
                UrunAdi = u.UrunAdi,
                Aktif = u.Aktif,
                Anasayfa = u.Anasayfa,
                KategoriAdi = u.Kategori.KategoriAdi, // EF bunu görünce otomatik join atar
            })
            .ToList();

        ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi", kategori);

        return View(urunler);
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

    public ActionResult Create()
    {
        // ViewBag.Kategoriler = _context.Kategoriler.ToList();
        ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi");
        // Buradaki selectlist sayesinde urun create sayfasında dropdown olacak  asp-items ile döngü yazmaya gerek yok.

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(UrunCreateModel model)
    {
        // Model de yapamadık. Çünkü bir resim dosyası yüklenebilir ama bu dosyanın boyutu 0 olabilir. Bu durumuda kontrol etmek için if blogu yazdık.
        if (model.Resim == null || model.Resim.Length == 0)
        {
            ModelState.AddModelError("Resim", "Resim zorunludur.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Kategoriler = new SelectList(
                _context.Kategoriler.ToList(),
                "Id",
                "KategoriAdi"
            );
            return View(model);
        }

        // upload edilen dosyanın ismi projenin içindeki bir dosya ile aynı olup onu ezmesin diye random bir dosya ismi olusturduk eğer istersek yüklenen dosyanın uzantısını da string metodlar ile alabiliriz.
        var fileName = Path.GetRandomFileName() + ".jpg";
        // upload edilen dosyanın fiziksel olarak hangi dizinde tutulacağını belirliyoruz
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
        // using içerisinde oluşturduğumuz FileStream nesnesi FileMode.create ile belirlediğimiz dizinde oluşturulacak olan dosyayı işaret eder. Bu nesne işlemden sonra kullanılmayacağı için işlem bittikten sonra otomatik dispose edilsin diye using blogu kullanılır. blogun içindeki kod ise gelen veriyi dosyaya kopyalar.
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await model.Resim!.CopyToAsync(stream);
        }

        var entity = new Urun()
        {
            UrunAdi = model.UrunAdi,
            Aciklama = model.Aciklama,
            Fiyat = model.Fiyat ?? 0, // model de fiyat null olabilir diye ?? kullandık boş bir değer geldiğinde null geliyordu ?? yaparak eğer null gelirse 0 degerini ver dedik. Bu yüzden model de fiyat nullable int olarak tanımlanmıştır
            Aktif = model.Aktif,
            Anasayfa = model.Anasayfa,
            KategoriId = (int)model.KategoriId!,
            Resim = fileName, // upload
        };

        _context.Urunler.Add(entity);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
        var entity = _context
            .Urunler.Select(u => new UrunEditModel
            {
                Id = u.Id,
                UrunAdi = u.UrunAdi,
                Aciklama = u.Aciklama,
                Fiyat = u.Fiyat,
                Aktif = u.Aktif,
                Anasayfa = u.Anasayfa,
                KategoriId = u.KategoriId,
                ResimAdi = u.Resim, // upload
            })
            .FirstOrDefault(u => u.Id == id);

        ViewBag.Kategoriler = new SelectList(_context.Kategoriler.ToList(), "Id", "KategoriAdi");

        return View(entity);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(int id, UrunEditModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Kategoriler = new SelectList(
                _context.Kategoriler.ToList(),
                "Id",
                "KategoriAdi"
            );
            return View(model);
        }

        if (id != model.Id)
        {
            return RedirectToAction("Index");
        }

        var entity = _context.Urunler.Find(model.Id);

        if (entity != null)
        {
            if (model.Resim != null)
            {
                var fileName = Path.GetRandomFileName() + ".jpg";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Resim!.CopyToAsync(stream);
                }

                entity.Resim = fileName;
            }

            entity.UrunAdi = model.UrunAdi;
            entity.Aciklama = model.Aciklama;
            entity.Fiyat = model.Fiyat ?? 0;
            entity.Aktif = model.Aktif;
            entity.Anasayfa = model.Anasayfa;
            entity.KategoriId = (int)model.KategoriId!;

            _context.SaveChanges();

            TempData["Mesaj"] = $"{entity.UrunAdi} urunu güncellendi";

            return RedirectToAction("Index");
        }

        return View(model);
    }

    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return RedirectToAction("Index");
        }

        var entity = _context.Urunler.FirstOrDefault(u => u.Id == id);

        if (entity != null)
        {
            return View(entity);
        }

        return View();
    }

    [HttpPost]
    public ActionResult DeleteConfirm(int? id)
    {
        if (id == null)
        {
            return RedirectToAction("Index");
        }

        var entity = _context.Urunler.FirstOrDefault(u => u.Id == id);

        if (entity != null)
        {
            _context.Remove(entity);
            _context.SaveChanges();

            TempData["Mesaj"] = $"{entity.UrunAdi} ürünü silindi";
        }

        return RedirectToAction("Index");
    }
}

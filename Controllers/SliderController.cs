using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Controllers;

[Authorize(Roles = "Admin")]
public class SliderController : BaseController
{
    private readonly DataContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SliderController(DataContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public ActionResult Index()
    {
        var sliderImages = _context
            .SliderImages.OrderBy(i => i.Index)
            .Select(i => new SliderGetModel
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                Baslik = i.Baslik,
                Aciklama = i.Aciklama,
                Index = i.Index,
                Aktif = i.Aktif,
            })
            .ToList();

        return View(sliderImages);
    }

    public ActionResult Create()
    {
        int currentIndex = _context.SliderImages.Any()
            ? _context.SliderImages.Max(i => i.Index) + 1
            : 1;

        var indexes = _context.SliderImages.Select(i => i.Index).ToList();

        indexes.Add(currentIndex);

        ViewBag.Indexes = new SelectList(indexes, currentIndex); // currentIndex varsayılan seçili gelir

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(SliderCreateModel model)
    {
        var fileName = Path.GetRandomFileName() + ".jpg";
        // upload edilen dosyanın fiziksel olarak hangi dizinde tutulacağını belirliyoruz
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

        //using içerisinde oluşturduğumuz FileStream nesnesi FileMode.create ile belirlediğimiz dizinde oluşturulacak olan dosyayı işaret eder. Bu nesne işlemden sonra kullanılmayacağı için işlem bittikten sonra otomatik dispose edilsin diye using blogu kullanılır. blogun içindeki kod ise gelen veriyi dosyaya kopyalar.
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await model.Resim.CopyToAsync(stream);
        }

        // Seçilen ındexin varolması durumunda kayıtlı sliderların ındexleri güncelleniyor.
        var indexes = _context.SliderImages.Select(i => i.Index).ToList();

        if (indexes.Contains(model.Index))
        {
            var sliders = _context.SliderImages.Where(i => i.Index >= model.Index).ToList();

            foreach (var slider in sliders)
            {
                slider.Index++;
            }

            _context.SaveChanges();
        }

        var entity = new Slider()
        {
            ImageUrl = fileName,
            Baslik = model.Baslik,
            Aciklama = model.Aciklama,
            Index = model.Index,
            Aktif = model.Aktif,
        };

        _context.SliderImages.Add(entity);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
        int currentIndex = _context.SliderImages.Max(i => i.Index) + 1;

        var indexes = _context.SliderImages.Select(i => i.Index).ToList();

        indexes.Add(currentIndex);

        ViewBag.Indexes = new SelectList(indexes);

        var entity = _context
            .SliderImages.Select(s => new SliderEditModel
            {
                Id = s.Id,
                ResimAdi = s.ImageUrl,
                Baslik = s.Baslik,
                Aciklama = s.Aciklama,
                Index = s.Index,
                Aktif = s.Aktif,
            })
            .FirstOrDefault(s => s.Id == id);

        return View(entity);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(int id, SliderEditModel model)
    {
        if (id != model.Id)
        {
            return RedirectToAction("Index");
        }

        var entity = _context.SliderImages.Find(model.Id);

        if (entity != null)
        {
            if (model.ResimDosyasi != null)
            {
                var fileName = Path.GetRandomFileName() + ".jpg";
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads",
                    fileName
                );

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ResimDosyasi!.CopyToAsync(stream);
                }

                entity.ImageUrl = fileName;

                TempData["Mesaj"] = $"{entity.Baslik} sliderı güncellendi";
            }

            var indexes = _context.SliderImages.Select(i => i.Index).ToList();

            if (indexes.Contains(model.Index))
            {
                // Index Küçültme
                if (entity.Index > model.Index)
                {
                    // -1 index
                    if (entity.Index - model.Index == 1)
                    {
                        var tinySlider = _context
                            .SliderImages.Where(i => i.Index == model.Index)
                            .ExecuteUpdate(i => i.SetProperty(x => x.Index, x => x.Index + 1));
                    }

                    // index - > 1
                    if (entity.Index - model.Index > 1)
                    {
                        var tinySliders = _context
                            .SliderImages.Where(i =>
                                i.Index >= model.Index && i.Index < entity.Index
                            )
                            .ExecuteUpdate(i => i.SetProperty(x => x.Index, x => x.Index + 1));
                    }
                }

                // Index Arttırma
                if (entity.Index < model.Index)
                {
                    // +1 index
                    if (model.Index - entity.Index == 1)
                    {
                        var bigSlider = _context
                            .SliderImages.Where(i => i.Index == model.Index)
                            .ExecuteUpdate(i => i.SetProperty(x => x.Index, x => x.Index - 1));
                    }

                    // index + > 1
                    if (model.Index - entity.Index > 1)
                    {
                        var bigSliders = _context
                            .SliderImages.Where(i =>
                                i.Index > entity.Index && i.Index <= model.Index
                            )
                            .ExecuteUpdate(i => i.SetProperty(x => x.Index, x => x.Index - 1));
                    }
                }
            }

            entity.Baslik = model.Baslik;
            entity.Aciklama = model.Aciklama;
            entity.Index = model.Index;
            entity.Aktif = model.Aktif;

            _context.SaveChanges();

            TempData["Mesaj"] = $"{model.Baslik} slider' ı güncellendi.";

            return RedirectToAction("Index");
        }

        return View(model);
    }

    public ActionResult Delete(int? id)
    {
        if (id != null)
        {
            var entity = _context.SliderImages.Find(id);

            return View(entity);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteConfirm(int? id)
    {
        if (id != null)
        {
            var query = _context.SliderImages.AsQueryable();

            var slider = query.FirstOrDefault(i => i.Id == id);

            if (slider != null)
            {
                // IWebHostEnvironment ASP.NET Core'un sunduğu bir interface'tir. Web uygulamasının çalıştığı ortam hakkında bilgi verir. Ve WebRootPath otomatik olarak projenin wwwroot klasörünün tam yolunu verir, elle yazmana gerek yok.
                var path = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    "uploads",
                    slider.ImageUrl
                );

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                query
                    .Where(i => i.Index > slider.Index)
                    .ExecuteUpdate(s => s.SetProperty(s => s.Index, s => s.Index - 1));

                _context.SliderImages.Remove(slider);
                _context.SaveChanges();

                TempData["Mesaj"] = $"{slider.Baslik} slider' ı silindi.";
            }
        }

        return RedirectToAction("Index");
    }
}

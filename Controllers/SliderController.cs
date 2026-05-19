using dotnet_store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dotnet_store.Controllers;

public class SliderController : Controller
{
    private readonly DataContext _context;

    public SliderController(DataContext context)
    {
        _context = context;
    }

    public ActionResult Index()
    {
        var sliderImages = _context
            .SliderImages.Select(i => new SliderGetModel
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
        int currentIndex = _context.SliderImages.Max(i => i.Index) + 1;

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
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

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
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

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
                var sliders = _context.SliderImages.Where(i => i.Index >= model.Index).ToList();

                foreach (var slider in sliders)
                {
                    slider.Index++;
                }

                _context.SaveChanges();
            }

            entity.Baslik = model.Baslik;
            entity.Aciklama = model.Aciklama;
            entity.Index = model.Index;
            entity.Aktif = model.Aktif;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(model);
    }
}

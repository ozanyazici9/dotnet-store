using dotnet_store.Data;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.ViewComponents;

public class Slider : ViewComponent
{
    private readonly DataContext _context;

    public Slider(DataContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var sliderImages = _context.SliderImages.OrderBy(i => i.Index).ToList();
        return View(sliderImages);
    }
}

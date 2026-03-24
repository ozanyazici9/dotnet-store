using dotnet_store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

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
        var sliderImages = _context.SliderImages.ToList();
        return View(sliderImages);
    }
}
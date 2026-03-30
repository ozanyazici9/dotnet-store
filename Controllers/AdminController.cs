using dotnet_store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Controllers;

public class AdminController : Controller
{
    private readonly DataContext _context;

    public AdminController(DataContext context)
    {
        _context = context;
    }

    public ActionResult Index()
    {
        return View();
    }
}

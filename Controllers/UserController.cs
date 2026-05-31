using dotnet_store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

public class UserController : Controller
{
    private UserManager<AppUser> _userManager;

    public UserController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public ActionResult Index()
    {
        return View(_userManager.Users);
    }
}

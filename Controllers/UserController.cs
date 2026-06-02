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

    public async Task<ActionResult> Index()
    {
        var users = new List<UserGetModel>();

        foreach (var user in _userManager.Users)
        {
            var userRole = await _userManager.GetRolesAsync(user);

            users.Add(
                new UserGetModel
                {
                    Id = user.Id,
                    AdSoyad = user.AdSoyad,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    UserRole = userRole.ToList(),
                }
            );
        }

        return View(users);
    }
}

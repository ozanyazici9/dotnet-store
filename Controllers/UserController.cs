using dotnet_store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dotnet_store.Controllers;

public class UserController : Controller
{
    private UserManager<AppUser> _userManager;
    private RoleManager<AppRole> _roleManager;

    public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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
                    UserRole = (List<string>)userRole,
                }
            );
        }

        return View(users);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(UserCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var newUser = new AppUser
            {
                UserName = model.Email,
                AdSoyad = model.AdSoyad,
                Email = model.Email,
            };
            var result = await _userManager.CreateAsync(newUser);

            if (result.Succeeded)
            {
                TempData["Mesaj"] = $"{model.Email} kullanıcısı eklendi.";
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(model);
    }

    public async Task<ActionResult> Edit(int id)
    {
        // User bilgisi ve kontrol
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return NotFound();

        // Model nesnesi oluşturma
        var model = new UserEditModel()
        {
            Id = user.Id,
            AdSoyad = user.AdSoyad,
            Email = user.Email!,
            UserRoles = (await _userManager.GetRolesAsync(user)).ToList(),
            AllRoles = _roleManager.Roles.Select(x => x.Name!).ToList(),
        };

        // View' a gönderme
        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(int id, UserEditModel model)
    {
        // Güvenlik ve validasyon kontrolleri
        if (id != model.Id)
            return RedirectToAction("Index");
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByIdAsync(id.ToString()!);
        if (user == null)
            return NotFound();

        // Rol Güncelleme
        var userRoles = await _userManager.GetRolesAsync(user);
        var rolesChanged = !userRoles
            .OrderBy(x => x)
            .SequenceEqual(model.SelectedRoles.OrderBy(x => x));

        if (rolesChanged)
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                foreach (var error in removeResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            var addResult = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
            if (!addResult.Succeeded)
            {
                foreach (var error in addResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }
        }

        // Kullanıcı Bilgilerini Güncelle
        user.AdSoyad = model.AdSoyad;
        user.Email = model.Email;
        user.UserName = model.Email;

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            foreach (var error in updateResult.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

        // Şifre Güncelleme
        if (!string.IsNullOrEmpty(model.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(user, token, model.Password);

            if (!passwordResult.Succeeded)
            {
                foreach (var error in passwordResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }
        }

        TempData["Mesaj"] = $"{user.UserName} kullanıcısı güncellendi.";
        return RedirectToAction("Index");
    }
}

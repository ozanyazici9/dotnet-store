using dotnet_store.Data;
using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace dotnet_store.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : BaseController
{
    private UserManager<AppUser> _userManager;
    private RoleManager<AppRole> _roleManager;

    public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ActionResult> Index(string role)
    {
        ViewBag.Roller = new SelectList(_roleManager.Roles, "Name", "Name", role);

        var model = new List<UserGetModel>();

        if (!string.IsNullOrEmpty(role))
        {
            var filteredUsers = await _userManager.GetUsersInRoleAsync(role);

            foreach (var u in filteredUsers)
            {
                var roles = await _userManager.GetRolesAsync(u);
                model.Add(
                    new UserGetModel
                    {
                        Id = u.Id,
                        AdSoyad = u.AdSoyad,
                        UserName = u.UserName!,
                        Email = u.Email!,
                        UserRole = roles.ToList(),
                    }
                );
            }

            return View(model);
        }

        foreach (var u in _userManager.Users.ToList())
        {
            var roles = await _userManager.GetRolesAsync(u);
            model.Add(
                new UserGetModel
                {
                    Id = u.Id,
                    AdSoyad = u.AdSoyad,
                    UserName = u.UserName!,
                    Email = u.Email!,
                    UserRole = roles.ToList(),
                }
            );
        }

        return View(model);
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

    public async Task<ActionResult> Delete(string? id)
    {
        if (id == null)
            return RedirectToAction("Index");

        var user = await _userManager.FindByIdAsync(id);

        if (id == null)
        {
            TempData["Mesaj"] = "Kullanıcı bulunamadı.";
            return RedirectToAction("Index");
        }

        return View(user);
    }

    [HttpPost]
    public async Task<ActionResult> DeleteConfirm(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            TempData["Mesaj"] = "Kullanıcı bulunamadı.";
            return RedirectToAction("Index");
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            TempData["Mesaj"] =
                "Kullanıcı silinemedi: "
                + string.Join(", ", result.Errors.Select(e => e.Description));
            return RedirectToAction("Index");
        }

        TempData["Mesaj"] = "Kullanıcı silindi.";

        return RedirectToAction("Index");
    }
}

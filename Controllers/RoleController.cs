using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dotnet_store.Controllers;

[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private RoleManager<AppRole> _roleManager;
    private UserManager<AppUser> _userManager;

    public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public ActionResult Index()
    {
        return View(_roleManager.Roles);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(RoleCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var role = new AppRole { Name = model.RoleAdi };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(model);
    }

    public async Task<ActionResult> Edit(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role != null)
        {
            var entity = new RoleEditModel { Id = role.Id, RoleAdi = role.Name! };
            return View(entity);
        }
        else
        {
            TempData["Mesaj"] = "Role mevcut değil";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<ActionResult> Edit(int id, RoleEditModel model)
    {
        if (id == model.Id)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());

                if (role != null)
                {
                    role.Name = model.RoleAdi;
                    role.NormalizedName = model.RoleAdi.ToUpper();

                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        TempData["Mesaj"] = $"{role.Name} rolü güncellendi.";
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Role Bulunamadı");
                }
            }

            return View(model);
        }

        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return RedirectToAction("Index");
        }

        var role = await _roleManager.FindByIdAsync(id.ToString()!);

        if (role != null)
        {
            ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name!);

            return View(role);
        }

        return RedirectToAction("Index");
    }

    public async Task<ActionResult> DeleteConfirm(int id, AppRole model)
    {
        if (id == model.Id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    TempData["Mesaj"] = $"{role.Name} rolü silindi.";
                    return RedirectToAction("Index");
                }
            }
        }

        TempData["Mesaj"] = "Rol silme işlemi başarısız.";
        return RedirectToAction("Index");
    }
}

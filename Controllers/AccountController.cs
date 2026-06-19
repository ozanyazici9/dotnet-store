using System.Security.Claims;
using AspNetCoreGeneratedDocument;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLitePCL;

namespace dotnet_store.Controllers;

public class AccountController : Controller
{
    private UserManager<AppUser> _userManager;
    private SignInManager<AppUser> _signInManager;
    private IEmailService _emailService;
    private readonly ICartService _cartService;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IEmailService emailService,
        ICartService cartService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _cartService = cartService;
    }

    public ActionResult Index()
    {
        return View();
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(AccountCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                AdSoyad = model.AdSoyad,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "User");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View();
    }

    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(AccountLoginModel model, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                await _signInManager.SignOutAsync();

                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.BeniHatirla,
                    true
                );

                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                    await _userManager.SetLockoutEndDateAsync(user, null);

                    await _cartService.TransferCartToUser(user.UserName!);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (result.IsLockedOut)
                {
                    var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                    var timeLeft = lockoutDate!.Value - DateTime.UtcNow;
                    ModelState.AddModelError(
                        "",
                        $"Hesabınız kilitlendi. Lütfen {timeLeft.Minutes + 1} dakika bekleyiniz."
                    );
                }
                else
                {
                    ModelState.AddModelError("", "Hatalı parola");
                }
            }
            else
            {
                ModelState.AddModelError("", "Hatalı email");
            }
        }
        return View(model);
    }

    [Authorize]
    public async Task<ActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [Authorize]
    public async Task<ActionResult> EditUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return NotFound();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return RedirectToAction("Login", "Account");

        var model = new AccountEditUserModel { AdSoyad = user.AdSoyad, Email = user.Email! };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> EditUser(AccountEditUserModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return NotFound();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return RedirectToAction("Login", "Account");

        user.AdSoyad = model.AdSoyad;
        user.Email = model.Email;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

        TempData["Mesaj"] = $"Bilgileriniz güncellendi.";

        return View(model);
    }

    public ActionResult AccessDenied()
    {
        return View();
    }

    [Authorize]
    public ActionResult ChangePassword()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> ChangePassword(AccountChangePasswordModel model)
    {
        // Güvenlik Kontrolü
        if (!ModelState.IsValid)
            return View();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return NotFound();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return RedirectToAction("Login", "Account");

        var result = await _userManager.ChangePasswordAsync(
            user,
            model.CurrentPassword,
            model.NewPassword
        );

        if (result.Succeeded)
        {
            TempData["Mesaj"] = "Parola Güncellendi";
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View();
    }

    public ActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> ForgotPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            TempData["Mesaj"] = "Eposta adresinizi giriniz";
            return View();
        }

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            TempData["Mesaj"] = "Bu eposta adresi mevcut değil";
            return View();
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // Eposta gönder
        var url = Url.Action("ResetPassword", "Account", new { userId = user.Id, token });

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var link = $"<a href='{baseUrl}{url}'>Şifre Yenile</a>";

        await _emailService.SendEmailAsync(user.Email!, "Parola Sıfırlama", link);

        TempData["Mesaj"] = "Eposta adresine gönderilen link ile şifreni sıfırlayabilirsin.";

        return RedirectToAction("Login");
    }

    public async Task<ActionResult> ResetPassword(string userId, string token)
    {
        if (userId == null || token == null)
        {
            TempData["Mesaj"] = "Kullanıcı bulunamadı. Lütfen tekrar giriş yapınız";
            return RedirectToAction("Login");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            TempData["Mesaj"] = "Kullanıcı bulunamadı. Lütfen tekrar giriş yapınız";
            return RedirectToAction("Login");
        }

        var model = new AccountResetPasswordModel { Token = token, Email = user.Email! };

        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> ResetPassword(AccountResetPasswordModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            TempData["Mesaj"] = "Kullanıcı Bulunamadı";
            return RedirectToAction("Login");
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

        TempData["Mesaj"] = "Parolanız Yenilendi";
        return RedirectToAction("Login");
    }
}

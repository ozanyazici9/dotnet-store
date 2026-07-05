using System.Security.Claims;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

public class AccountController : BaseController
{
    private UserManager<AppUser> _userManager;
    private SignInManager<AppUser> _signInManager;
    private readonly IAccountService _accountService;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IAccountService accountService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _accountService = accountService;
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
        if (!ModelState.IsValid)
            return View(model);

        var result = await _accountService.Login(model.Email, model.Password, model.BeniHatirla);

        if (result.Succeeded)
            return !string.IsNullOrEmpty(returnUrl)
                ? Redirect(returnUrl)
                : RedirectToAction("Index", "Home");

        if (result.IsLockedOut)
            ModelState.AddModelError(
                "",
                $"Hesabınız kilitlendi {result.LockoutMinutesLeft} dakika bekleyiniz"
            );
        else
            ModelState.AddModelError("", result.ErrorMessage!);

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
        var userId = GetUserId();

        var user = await _userManager.FindByIdAsync(userId!);
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

        var result = await _accountService.EditUser(model.AdSoyad, model.Email, GetUserId()!);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(model);
        }

        TempData["Mesaj"] = $"Bilgileriniz güncellendi.";

        return RedirectToAction("EditUser", "Account");
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
            return View(model);

        var result = await _accountService.ChangePassword(
            model.CurrentPassword,
            model.NewPassword,
            GetCustomerUserName()
        );

        if (result.Succeeded)
        {
            TempData["Mesaj"] = "Parola Güncellendi";
            return View();
        }
        else
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View();
        }
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

        // Eposta gönder
        var resetLink = Url.Action("ResetPassword", "Account", null, Request.Scheme);
        var result = await _accountService.ForgotPassword(email, resetLink!);

        if (!result.Succeeded)
        {
            TempData["Mesaj"] = result.ErrorMessage!;
            return View();
        }

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

        var result = await _accountService.ResetPassword(
            model.Email,
            model.Token,
            model.NewPassword
        );

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(model);
        }

        TempData["Mesaj"] = "Parolanız Yenilendi";
        return RedirectToAction("Login");
    }
}

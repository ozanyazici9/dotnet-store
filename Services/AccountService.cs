using dotnet_store.Data;
using dotnet_store.Exceptions;
using dotnet_store.Models;
using dotnet_store.Services.Results;
using Microsoft.AspNetCore.Identity;

namespace dotnet_store.Services;

public interface IAccountService
{
    Task<LoginResult> Login(string email, string password, bool beniHatirla);
    Task<ServiceResult> ChangePassword(
        string currentPassword,
        string newPassword,
        string customerId
    );
    Task<ServiceResult> ForgotPassword(string email, string url);
    Task<ServiceResult> ResetPassword(string email, string token, string newPassword);
    Task<ServiceResult> EditUser(string adSoyad, string email, string userId);
}

public class AccountService : IAccountService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ICartService _cartService;
    private readonly IEmailService _emailService;

    public AccountService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ICartService cartService,
        IEmailService emailService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _cartService = cartService;
        _emailService = emailService;
    }

    public async Task<ServiceResult> ChangePassword(
        string currentPassword,
        string newPassword,
        string customerId
    )
    {
        var user = await _userManager.FindByIdAsync(customerId);
        if (user == null)
            throw new NotFoundException("Kullanıcı bulunamadı.");

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (result.Succeeded)
            return ServiceResult.Success();
        else
            return ServiceResult.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<ServiceResult> ForgotPassword(string email, string resetLink)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            throw new NotFoundException("Kullanıcı bulunamadı.");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var link = $"<a href='{resetLink}?userId={user.Id}&token={token}'>Şifre Yenile</a>";

        await _emailService.SendEmailAsync(user.Email!, "Parola Sıfırlama", link);
        return ServiceResult.Success();
    }

    public async Task<LoginResult> Login(string email, string password, bool beniHatirla)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            throw new NotFoundException("Kullanıcı bulunamadı.");

        await _signInManager.SignOutAsync();

        var result = await _signInManager.PasswordSignInAsync(user, password, beniHatirla, true);

        if (result.Succeeded)
        {
            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, null);

            await _cartService.TransferCartToUser(user.UserName!);

            return new LoginResult { Succeeded = true };
        }
        else if (result.IsLockedOut)
        {
            var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
            var timeLeft = lockoutDate!.Value - DateTime.UtcNow;

            return new LoginResult
            {
                LockoutMinutesLeft = (int)timeLeft.TotalMinutes,
                IsLockedOut = true,
            };
        }
        else
        {
            return new LoginResult { ErrorMessage = " Şifre yanlış." };
        }
    }

    public async Task<ServiceResult> ResetPassword(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new NotFoundException("Kullanıcı bulunamadı.");

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
            return ServiceResult.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> EditUser(string adSoyad, string email, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new NotFoundException("Kullanıcı bulunamadı.");

        user.AdSoyad = adSoyad;
        user.Email = email;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return ServiceResult.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

        return ServiceResult.Success();
    }
}

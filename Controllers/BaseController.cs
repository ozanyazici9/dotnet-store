using System.Security.Claims;
using dotnet_store.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

public class BaseController : Controller
{
    protected string GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new NotFoundException("Kullanıcı bulunamadı.");

        return userId;
    }

    /// <summary>
    /// Giriş yapmamış kullanıcılar için cookie'den okur.
    /// </summary>
    protected string GetCustomerUserName()
    {
        return User.Identity!.Name ?? Request.Cookies["customerId"]!;
    }
}

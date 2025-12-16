using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Model.Account;

namespace ToDoApp.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IAccountService accountService,
        ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    #region Register

    [HttpGet]
    public IActionResult Register()
        => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        RegisterUserDto data,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(data);

        var result = await _accountService.Register(data, cancellationToken);

        if (result.IsSuccess)
        {
            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(Login));
        }

        TempData["ErrorMessage"] = result.Message;
        if (result.Errors.Any())
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        return View(data);
    }

    #endregion

    #region Login

    [HttpGet]
    public IActionResult Login(string message)
    {
        ViewBag.Message = message;
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginUserDto data,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(data);

        var result = await _accountService.Login(data, cancellationToken);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = result.Message;
            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return View(data);
        }

        #region Login User Into Identity

        var user = result.Data;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.MobilePhone, user.PhoneNumber),
            new(ClaimTypes.Name, user.UserName),
        };

        var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(claimIdentity);

        var authProps = new AuthenticationProperties
        {
            IsPersistent = true
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

        #endregion

        _logger.LogInformation("User {PhoneNumber} logged in successfully", user.PhoneNumber);

        return RedirectToAction("Index", "Home");
    }

    #endregion

    #region Logout

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        _logger.LogInformation("User logged out");
        return Redirect("/");
    }

    #endregion
}

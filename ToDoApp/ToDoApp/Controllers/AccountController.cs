using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoApp.Domain.Entities.Account;
using ToDoApp.Domain.Model.Account;
using ToDoApp.Infra;

namespace ToDoApp.Controllers;

public class AccountController(
    ApplicationDbContext context) :
    Controller
{
    #region Register

    [HttpGet]
    public IActionResult Register()
        => View();

    [HttpPost , ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
        RegisterUserDto data ,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(ModelState);

        //User duplication checker
        if(await context.Users.AnyAsync(user=> user.PhoneNumber == data.PhoneNumber))
            return View(ModelState);

        //Add user record to the data base 
        var newUser = new UserEntity()
        {
            CreateDate = DateTime.Now,
            Email = null,
            IsDelete = false , 
            Password = data.Password ,
            PhoneNumber = data.PhoneNumber,
            UserName = data.PhoneNumber
        };

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Login));
    }

    #endregion

    #region Login

    [HttpGet]
    public IActionResult Login(string message)
    {
        ViewBag.Message = message;
       return View();
    }

    [HttpPost , ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        LoginUserDto data , 
        CancellationToken cancellationToken)
    {
        //Exist phone number checker
        if (!await context.Users.AnyAsync(user => user.PhoneNumber == data.PhoneNumber))
            return RedirectToAction(nameof(Login) , "Account" , new { message = "کاربری با اطلاعات وارد شده یافت نشد." });

        //Password checker
        if (!await context.Users.AnyAsync(user => user.PhoneNumber == data.PhoneNumber && user.Password == data.Password))
            return View("کلمه ی ورود ارسالی اشتباه است.");

        //Set Coockie for user

        #region Login User Into Identity

        var user = await context.Users.FirstOrDefaultAsync(p=> p.PhoneNumber.Equals(data.PhoneNumber));

        var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.MobilePhone, user.PhoneNumber),
                new (ClaimTypes.Name, user.UserName),
            };

        var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(claimIdentity);

        var authProps = new AuthenticationProperties();
        authProps.IsPersistent = true;

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

        #endregion

        return RedirectToAction("Index", "Home");
    }

    #endregion

    #region Logout

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }

    #endregion
}

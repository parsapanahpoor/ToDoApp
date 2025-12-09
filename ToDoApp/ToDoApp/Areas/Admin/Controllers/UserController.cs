using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Services.Account;
using ToDoApp.Domain.Model.Account;
using ToDoApp.HttpManager;

namespace ToDoApp.Areas.Admin.Controllers;

public class UserController(
    UserService userService) :
    AdminBaseController
{
    #region Filter Users

    public async Task<IActionResult> FilterUsers(FilterUsersDto filter)
    {
        var users = await userService.FilterUsers(filter);
        return View(users);
    }

    #endregion

    #region Create User

    public async Task<IActionResult> CreateUser()
    {
        ViewBag.Roles = await userService.GetAllRoles();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = await userService.GetAllRoles();
            TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی‌باشد";
            return View(model);
        }

        var result = await userService.CreateUser(model);
        if (result)
        {
            TempData[SuccessMessage] = "کاربر با موفقیت ایجاد شد";
            return RedirectToAction(nameof(FilterUsers));
        }

        ViewBag.Roles = await userService.GetAllRoles();
        TempData[WarningMessage] = "نام کاربری یا ایمیل تکراری است";
        return View(model);
    }

    #endregion

    #region Edit User

    [HttpGet]
    public async Task<IActionResult> EditUser(ulong id)
    {
        var user = await userService.FillEditUserDto(id);
        if (user == null)
        {
            TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
            return RedirectToAction(nameof(FilterUsers));
        }

        ViewBag.Roles = await userService.GetAllRoles();
        return View(user);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(EditUserDto model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = await userService.GetAllRoles();
            TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی‌باشد";
            return View(model);
        }

        var result = await userService.EditUser(model);
        if (result)
        {
            TempData[SuccessMessage] = "کاربر با موفقیت ویرایش شد";
            return RedirectToAction(nameof(FilterUsers));
        }

        ViewBag.Roles = await userService.GetAllRoles();
        TempData[WarningMessage] = "نام کاربری یا ایمیل تکراری است";
        return View(model);
    }

    #endregion

    #region Delete User

    public async Task<IActionResult> DeleteUser(ulong userId)
    {
        var result = await userService.DeleteUser(userId);
        if (result)
            return JsonResponseStatus.Success();

        return JsonResponseStatus.Error();
    }

    #endregion

    #region User Roles

    public async Task<IActionResult> UserRoles(ulong userId)
    {
        var userRoles = await userService.GetUserRoles(userId);
        if (userRoles == null)
        {
            TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
            return RedirectToAction(nameof(FilterUsers));
        }

        return View(userRoles);
    }

    #endregion
}


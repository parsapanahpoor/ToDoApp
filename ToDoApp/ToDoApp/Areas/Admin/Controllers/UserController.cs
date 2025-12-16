using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Model.Account;
using ToDoApp.HttpManager;

namespace ToDoApp.Areas.Admin.Controllers;

public class UserController : AdminBaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    #region Filter Users

    public async Task<IActionResult> FilterUsers(FilterUsersDto filter, CancellationToken cancellationToken)
    {
        var users = await _userService.FilterUsers(filter, cancellationToken);
        return View(users);
    }

    #endregion

    #region Create User

    public async Task<IActionResult> CreateUser(CancellationToken cancellationToken)
    {
        ViewBag.Roles = await _userService.GetAllRoles(cancellationToken);
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = await _userService.GetAllRoles(cancellationToken);
            TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی‌باشد";
            return View(model);
        }

        var result = await _userService.CreateUser(model, cancellationToken);
        
        if (result.IsSuccess)
        {
            TempData[SuccessMessage] = result.Message;
            return RedirectToAction(nameof(FilterUsers));
        }

        ViewBag.Roles = await _userService.GetAllRoles(cancellationToken);
        TempData[WarningMessage] = result.Message;
        if (result.Errors.Any())
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
        
        return View(model);
    }

    #endregion

    #region Edit User

    [HttpGet]
    public async Task<IActionResult> EditUser(ulong id, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserForEdit(id, cancellationToken);
        
        if (!result.IsSuccess)
        {
            TempData[ErrorMessage] = result.Message;
            return RedirectToAction(nameof(FilterUsers));
        }

        ViewBag.Roles = await _userService.GetAllRoles(cancellationToken);
        return View(result.Data);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(EditUserDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = await _userService.GetAllRoles(cancellationToken);
            TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی‌باشد";
            return View(model);
        }

        var result = await _userService.EditUser(model, cancellationToken);
        
        if (result.IsSuccess)
        {
            TempData[SuccessMessage] = result.Message;
            return RedirectToAction(nameof(FilterUsers));
        }

        ViewBag.Roles = await _userService.GetAllRoles(cancellationToken);
        TempData[WarningMessage] = result.Message;
        if (result.Errors.Any())
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
        
        return View(model);
    }

    #endregion

    #region Delete User

    public async Task<IActionResult> DeleteUser(ulong userId, CancellationToken cancellationToken)
    {
        var result = await _userService.DeleteUser(userId, cancellationToken);
        
        if (result.IsSuccess)
            return JsonResponseStatus.Success();

        return JsonResponseStatus.Error();
    }

    #endregion

    #region User Roles

    public async Task<IActionResult> UserRoles(ulong userId, CancellationToken cancellationToken)
    {
        var userRoles = await _userService.GetUserRoles(userId, cancellationToken);
        
        if (userRoles == null)
        {
            TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
            return RedirectToAction(nameof(FilterUsers));
        }

        return View(userRoles);
    }

    #endregion
}


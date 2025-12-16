using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Model.Account;
using ToDoApp.HttpManager;

namespace ToDoApp.Areas.Admin.Controllers;

public class AccessController : AdminBaseController
{
    private readonly IRoleService _roleService;

    public AccessController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    #region Filter roles

    public async Task<IActionResult> FilterRoles(FilterRolesDto filter, CancellationToken cancellationToken)
        => View(await _roleService.FilterRoles(filter, cancellationToken));

    #endregion

    #region Create role

    public IActionResult CreateRole()
        => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRole(CreateRoleDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "داده‌های وارد شده معتبر نیستند";
            return View(model);
        }

        var result = await _roleService.CreateRole(model, cancellationToken);
        
        if (result.IsSuccess)
        {
            TempData[SuccessMessage] = result.Message;
            return RedirectToAction(nameof(FilterRoles));
        }

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

    #region Edit role

    [HttpGet]
    public async Task<IActionResult> EditRole(ulong id, CancellationToken cancellationToken)
    {
        var result = await _roleService.GetRoleForEdit(id, cancellationToken);
        
        if (!result.IsSuccess)
        {
            TempData[ErrorMessage] = result.Message;
            return RedirectToAction(nameof(FilterRoles));
        }

        return View(result.Data);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRole(EditRoleDto edit, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "داده‌های وارد شده معتبر نیستند";
            return View(edit);
        }

        var result = await _roleService.EditRole(edit, cancellationToken);
        
        if (result.IsSuccess)
        {
            TempData[SuccessMessage] = result.Message;
            return RedirectToAction(nameof(FilterRoles));
        }

        TempData[WarningMessage] = result.Message;
        if (result.Errors.Any())
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
        
        return View(edit);
    }

    #endregion

    #region Delete role

    public async Task<IActionResult> DeleteRole(ulong roleId, CancellationToken cancellationToken)
    {
        var result = await _roleService.DeleteRole(roleId, cancellationToken);
        
        if (result.IsSuccess)
            return JsonResponseStatus.Success();

        return JsonResponseStatus.Error();
    }

    #endregion
}

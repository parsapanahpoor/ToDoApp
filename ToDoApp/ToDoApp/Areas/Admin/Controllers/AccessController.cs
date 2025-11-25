using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Services.Account;
using ToDoApp.Domain.Model.Account;
using ToDoApp.HttpManager;

namespace ToDoApp.Areas.Admin.Controllers;

public class AccessController(
    RoleService roleService) : 
    AdminBaseController
{
    #region Filter roles

    public async Task<IActionResult> FilterRoles(FilterRolesDto filter)
        => View(await roleService.FilterRoles(filter));

    #endregion

    #region Create role

    public IActionResult CreateRole()
        => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRole(CreateRoleDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "Entered data was not Valid!";
            return View(model);
        }

        var result = await roleService.CreateRole(model);
        if (result)
        {
            TempData[SuccessMessage] = "Operation was success";
            return RedirectToAction(nameof(FilterRoles));
        }

        TempData[WarningMessage] = "Operation had an error!";
        return View(model);
    }

    #endregion

    #region Edit role

    [HttpGet]
    public async Task<IActionResult> EditRole(ulong id)
    {
        var result = await roleService.FillEditRoleDto(id);
        if (result == null)
            return NotFound();

        return View(result);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRole(EditRoleDto edit)
    {
        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "Input values ​​are not valid";
            return View(edit);
        }

        var result = await roleService.EditRole(edit);
        if(result)
        {
                TempData[SuccessMessage] ="mission accomplished";
                return RedirectToAction(nameof(FilterRoles));
        }

        TempData[WarningMessage] = "Operation had an error!";
        return View(edit);
    }

    #endregion

    #region Delete role

    public async Task<IActionResult> DeleteRole(ulong roleId)
    {
        var result = await roleService.DeleteRole(roleId);
        if (result)
            return JsonResponseStatus.Success();

        return JsonResponseStatus.Error();
    }

    #endregion
}

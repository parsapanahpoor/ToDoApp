using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Services.Account;

namespace ToDoApp.Areas.Admin.ViewComponents;

public class RoleSelectionViewComponent : ViewComponent
{
    private readonly UserService _userService;

    public RoleSelectionViewComponent(UserService userService)
    {
        _userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync(List<ulong> selectedRoles = null)
    {
        var roles = await _userService.GetAllRoles();
        ViewBag.SelectedRoles = selectedRoles ?? new List<ulong>();
        return View(roles);
    }
}


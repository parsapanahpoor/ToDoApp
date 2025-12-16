using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Interfaces;

namespace ToDoApp.Areas.Admin.ViewComponents;

public class RoleSelectionViewComponent : ViewComponent
{
    private readonly IUserService _userService;

    public RoleSelectionViewComponent(IUserService userService)
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


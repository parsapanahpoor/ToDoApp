using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Areas.Admin.ViewComponents;

public class AdminSideBarViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    => View("AdminSideBar");
}

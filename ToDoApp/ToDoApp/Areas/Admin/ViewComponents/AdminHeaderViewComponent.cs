using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Areas.Admin.ViewComponents;

public class AdminHeaderViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    => View("AdminHeader");
}

using Microsoft.AspNetCore.Mvc;
namespace ToDoApp.Areas.Admin.Controllers;

public class HomeController : 
    AdminBaseController
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    => View();
}

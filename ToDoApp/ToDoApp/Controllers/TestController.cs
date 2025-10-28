using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Controllers;

public class TestController : Controller
{
    [HttpGet]
    public IActionResult ShowSumPage(int a, string b)
    {
        return View();
    }

    [HttpPost]
    public IActionResult Sum1(int a , string b)
    {
        //Logic and business

        return RedirectToAction(nameof(ShowSumPage));
    }

    [HttpGet]
    public IActionResult Sum2(int a, string b)
    {
        //Logic and business

        return RedirectToAction(nameof(ShowSumPage));
    }
}

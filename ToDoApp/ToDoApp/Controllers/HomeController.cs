using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public void Sum(int a , int b)
        {
            
        }

        public IActionResult Test()
        {
            return View();
        }
    }
}

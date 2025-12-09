using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Application.Extensions;

public static class ControllerExtensions
{
    public static string GetCurrentLanguage(this Controller controller)
    {
        return controller.HttpContext.Items["Language"]?.ToString() ?? "fa";
    }

    public static bool IsPersian(this Controller controller)
    {
        return GetCurrentLanguage(controller) == "fa";
    }
}


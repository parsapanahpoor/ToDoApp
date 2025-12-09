namespace ToDoApp.Application.Middleware;

public class LanguageMiddleware
{
    private readonly RequestDelegate _next;

    public LanguageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // بررسی زبان از Query String یا Cookie
        var language = context.Request.Query["lang"].ToString();
        
        if (string.IsNullOrEmpty(language))
        {
            language = context.Request.Cookies["Language"] ?? "fa";
        }
        else
        {
            // ذخیره زبان در Cookie
            context.Response.Cookies.Append("Language", language, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddYears(1)
            });
        }

        // تنظیم Culture
        var culture = language == "en" ? "en-US" : "fa-IR";
        var cultureInfo = new System.Globalization.CultureInfo(culture);
        
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        // ذخیره زبان در ViewBag (از طریق HttpContext.Items)
        context.Items["Language"] = language;
        context.Items["Culture"] = cultureInfo;

        await _next(context);
    }
}

public static class LanguageMiddlewareExtensions
{
    public static IApplicationBuilder UseLanguageMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LanguageMiddleware>();
    }
}


using System.Globalization;

namespace ToDoApp.Application.Middleware;

public class LanguageMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly string[] SupportedLanguages = { "fa", "en" };
    private const string DefaultLanguage = "fa";
    private const string LanguageCookieName = "ToDoApp.Language";

    public LanguageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Get language from query string, cookie, or default
        var language = GetLanguage(context);

        // Set culture
        var culture = new CultureInfo(language);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        // Store in HttpContext for easy access
        context.Items["Language"] = language;
        context.Items["Culture"] = culture;
        context.Items["IsRTL"] = language == "fa";

        // If language is specified in query string, update cookie
        if (context.Request.Query.ContainsKey("lang"))
        {
            var langFromQuery = context.Request.Query["lang"].ToString().ToLower();
            if (SupportedLanguages.Contains(langFromQuery))
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Path = "/"
                };
                context.Response.Cookies.Append(LanguageCookieName, langFromQuery, cookieOptions);
            }
        }

        await _next(context);
    }

    private string GetLanguage(HttpContext context)
    {
        // 1. Check query string
        if (context.Request.Query.ContainsKey("lang"))
        {
            var lang = context.Request.Query["lang"].ToString().ToLower();
            if (SupportedLanguages.Contains(lang))
                return lang;
        }

        // 2. Check cookie
        if (context.Request.Cookies.TryGetValue(LanguageCookieName, out var cookieLang))
        {
            if (SupportedLanguages.Contains(cookieLang.ToLower()))
                return cookieLang.ToLower();
        }

        // 3. Check Accept-Language header
        var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();
        if (!string.IsNullOrEmpty(acceptLanguage))
        {
            var languages = acceptLanguage.Split(',')
                .Select(x => x.Split(';')[0].Trim().ToLower())
                .ToArray();

            foreach (var lang in languages)
            {
                if (lang.StartsWith("fa"))
                    return "fa";
                if (lang.StartsWith("en"))
                    return "en";
            }
        }

        // 4. Return default
        return DefaultLanguage;
    }
}

// Extension method for easy middleware registration
public static class LanguageMiddlewareExtensions
{
    public static IApplicationBuilder UseLanguageMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LanguageMiddleware>();
    }
}


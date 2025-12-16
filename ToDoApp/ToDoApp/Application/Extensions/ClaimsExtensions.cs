using System.Security.Claims;

namespace ToDoApp.Application.Extensions;

public static class ClaimsExtensions
{
    public static ulong GetUserId(this ClaimsPrincipal user)
    {
        if (user == null || !user.Identity.IsAuthenticated)
            return 0;

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim != null && ulong.TryParse(userIdClaim.Value, out ulong userId))
            return userId;

        return 0;
    }

    public static string GetPhoneNumber(this ClaimsPrincipal user)
    {
        if (user == null || !user.Identity.IsAuthenticated)
            return null;

        return user.FindFirst(ClaimTypes.MobilePhone)?.Value;
    }

    public static string GetUserName(this ClaimsPrincipal user)
    {
        if (user == null || !user.Identity.IsAuthenticated)
            return null;

        return user.FindFirst(ClaimTypes.Name)?.Value;
    }
}

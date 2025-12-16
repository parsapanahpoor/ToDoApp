using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ToDoApp.Application.Extensions;
using ToDoApp.Domain.Interfaces;

namespace ToDoApp.Areas.Admin.ActionFilterAttributes;

public class CheckUserHasAnyRole : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userRoleRepository = (IUserRoleRepository)context.HttpContext.RequestServices.GetService(typeof(IUserRoleRepository))!;

        var userId = context.HttpContext.User.GetUserId();

        if (userId == 0)
        {
            context.Result = new RedirectResult("/Account/Login");
            return;
        }

        var hasUserAnyRole = await userRoleRepository.HasUserAnyRoleAsync(userId, default);

        if (!hasUserAnyRole)
        {
            context.Result = new RedirectResult("/");
            return;
        }

        await next();
    }
}

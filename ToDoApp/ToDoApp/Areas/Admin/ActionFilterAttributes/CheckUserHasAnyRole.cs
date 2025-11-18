using Microsoft.AspNetCore.Mvc.Filters;
namespace ToDoApp.Areas.Admin.ActionFilterAttributes;
public class CheckUserHasAnyRole : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //var service = (IUserRepository)context.HttpContext.RequestServices.GetService(typeof(IUserQueryRepository))!;

        //base.OnActionExecuting(context);

        ////var hasUserAnyRole = service.IsUserAdmin(context.HttpContext.User.GetUserId() , default).Result;
        //var hasUserAnyRole = true;

        //if (!hasUserAnyRole)
        //{
        //    context.HttpContext.Response.Redirect("/");
        //}
    }
}

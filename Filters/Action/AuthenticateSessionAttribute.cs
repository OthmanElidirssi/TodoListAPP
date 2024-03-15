using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TodoListAPP.Filters.Action
{

    public class AuthenticateSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userId = session.GetString("UserId");
            var userName = session.GetString("UserName");

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
            {

                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }

}
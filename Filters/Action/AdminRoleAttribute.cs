using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoListAPP.Filters.Action
{
    public class AdminRoleAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userRole = session.GetString("UserRole");


            if (userRole != "ROLE_ADMIN")
            {

                context.Result = new RedirectToActionResult("Home", "User", null);
            }
        }
    }
}

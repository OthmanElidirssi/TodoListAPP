using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoListAPP.Filters.Action
{
    public class TestFilter:ActionFilterAttribute
    {


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.Result = new RedirectResult("~/Home/Index");

            base.OnActionExecuting(context);
        }
    }
}

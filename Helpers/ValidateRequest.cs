using Microsoft.AspNetCore.Mvc.Filters;

namespace UserService.Helpers;

public class ValidateRequest : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {   
        if (!context.ModelState.IsValid)
        {
            context.Result = new ValidationFailedResult(context.ModelState);
        }
    }
}
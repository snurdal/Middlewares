using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Middlewares.Handlers
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new
                    {
                        field = e.Key,
                        message = e.Value.Errors.Select(x => x.ErrorMessage).ToArray()
                    });
                context.Result = new JsonResult(
                    new
                    {
                        Error = "Validation Error",
                        Message = "One or more validation errors occurred.",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Details = errors
                    });

                return;
            }
        }
    }
}

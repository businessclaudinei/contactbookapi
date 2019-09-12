using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

public class ValidatorFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if(!context.ModelState.IsValid)
        {
            var errorsInState = context.ModelState.Where(v=>v.Value.Errors.Any())
            .SelectMany(e=> e.Value.Errors.Select(m=> new ErrorModel{
                FieldName = e.Key,
                Message = m.ErrorMessage
            }));
            context.Result= new BadRequestObjectResult(new ErrorResponse{Errors = errorsInState});
            return;
        }
        await next();
    }
}

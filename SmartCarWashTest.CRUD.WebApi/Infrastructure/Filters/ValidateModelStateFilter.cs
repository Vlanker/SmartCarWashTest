using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartCarWashTest.CRUD.WebApi.Infrastructure.Responses;

namespace SmartCarWashTest.CRUD.WebApi.Infrastructure.Filters
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var validationErrors = context.ModelState
                .Keys
                .SelectMany(key => context.ModelState[key].Errors)
                .Select(error => error.ErrorMessage)
                .ToArray();

            var json = new JsonErrorResponse(validationErrors);

            context.Result = new BadRequestObjectResult(json);
        }
    }
}
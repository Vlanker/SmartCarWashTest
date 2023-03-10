using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using SmartCarWashTest.CRUD.WebApi.Infrastructure.Middlewares.Requirements;

namespace SmartCarWashTest.CRUD.WebApi.Infrastructure.Middlewares.Handlers
{
    public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

        public async Task HandleAsync(
            RequestDelegate next,
            HttpContext context,
            AuthorizationPolicy policy,
            PolicyAuthorizationResult authorizeResult)
        {
            // If the authorization was forbidden and the resource had a specific requirement,
            // provide a custom 404 response.
            if (authorizeResult.Forbidden
                && authorizeResult.AuthorizationFailure!.FailedRequirements
                    .OfType<Show404Requirement>().Any())
            {
                // Return a 404 to make it appear as if the resource doesn't exist.
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            // Fall back to the default implementation.
            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
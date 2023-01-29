using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace SmartCarWashTest.CRUD.WebApi.Infrastructure.Middlewares.Headers
{
    public class SecurityHeaders
    {
        private readonly RequestDelegate _next;

        public SecurityHeaders(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            // add any HTTP response headers you want here
            context.Response.Headers.Add(
                "super-secure", new StringValues("enable"));
            return _next(context);
        }
    }
}
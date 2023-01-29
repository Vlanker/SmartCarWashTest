using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SmartCarWashTest.CRUD.WebApi.Infrastructure.ActionResults
{
    /// <summary>
    /// Internal server error object result.
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        /// <summary>
        /// Create <see cref="InternalServerErrorObjectResult"/>.
        /// </summary>
        /// <param name="error"></param>
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
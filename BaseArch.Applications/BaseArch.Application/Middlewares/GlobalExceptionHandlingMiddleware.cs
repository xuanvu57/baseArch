using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BaseArch.Application.Middlewares
{
    /// <summary>
    /// Middleware for global exception handler to handle the unhandle exceptions
    /// </summary>
    /// <param name="next"><see cref="RequestDelegate"/></param>
    /// <param name="logger"><see cref="ILogger"/></param>
    public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        /// <summary>
        /// Handle the middleware
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Unhandle exception occurred: {UnhandleExceptionMessage}", exception.Message);
            }
        }
    }
}

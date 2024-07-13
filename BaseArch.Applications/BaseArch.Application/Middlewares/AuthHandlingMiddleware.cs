using BaseArch.Domain.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BaseArch.Application.Middlewares
{
    /// <summary>
    /// Middleware to handle responses from unauthenticated and unauthorized problems
    /// </summary>
    /// <param name="next"><see cref="RequestDelegate"/></param>
    public class AuthHandlingMiddleware(RequestDelegate next)
    {
        /// <summary>
        /// Handle the response by status code
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContent"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            await next(httpContext);

            switch (httpContext.Response.StatusCode)
            {
                case (int)HttpStatusCode.Unauthorized:
                    await UnauthorizedHandler(httpContext);
                    break;
                case (int)HttpStatusCode.Forbidden:
                    await ForbiddenHandler(httpContext);
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Handle unauthorizied problem
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContent"/></param>
        /// <returns><see cref="Task"/></returns>
        private static async Task UnauthorizedHandler(HttpContext httpContext)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Type = ProblemDetailsTypeConst.Type401Unauthorized,
                Title = HttpStatusCode.Unauthorized.ToString(),
                Status = StatusCodes.Status401Unauthorized,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }

        /// <summary>
        /// Handle unauthenticated problem
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContent"/></param>
        /// <returns><see cref="Task"/></returns>
        private static async Task ForbiddenHandler(HttpContext httpContext)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Type = ProblemDetailsTypeConst.Type403Forbidden,
                Title = HttpStatusCode.Forbidden.ToString(),
                Status = StatusCodes.Status403Forbidden,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}

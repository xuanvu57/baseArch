using BaseArch.Domain.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BaseArch.Application.Middlewares
{
    public class AuthMiddleware(RequestDelegate next)
    {
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

        private async Task UnauthorizedHandler(HttpContext httpContext)
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

        private async Task ForbiddenHandler(HttpContext httpContext)
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

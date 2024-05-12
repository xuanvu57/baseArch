using BaseArch.Application.ExceptionHandlers;
using Microsoft.AspNetCore.Builder;

namespace BaseArch.Application.Extensions
{
    /// <summary>
    /// Extension to register the global exception handler
    /// </summary>
    public static class GlobalExceptionHandlingMiddlewareRegistration
    {
        /// <summary>
        /// Use the global unhandled exception
        /// </summary>
        /// <param name="app"><see cref="WebApplication"/></param>
        public static void UserGlobalExceptionHandlingMiddlewareRegistration(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}

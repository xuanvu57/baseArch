using BaseArch.Application.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace BaseArch.Application.Registrations
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
        public static void UserGlobalExceptionHandlingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}

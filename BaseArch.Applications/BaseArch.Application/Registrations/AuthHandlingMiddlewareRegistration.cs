using BaseArch.Application.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace BaseArch.Application.Registrations
{
    /// <summary>
    /// Extension to register <see cref="AuthHandlingMiddleware"/> middleware 
    /// </summary>
    public static class AuthHandlingMiddlewareRegistration
    {
        /// <summary>
        /// Register to use <see cref="AuthHandlingMiddleware"/> middleware
        /// </summary>
        /// <param name="app"></param>
        public static void UserAuthHandlingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<AuthHandlingMiddleware>();
        }
    }
}

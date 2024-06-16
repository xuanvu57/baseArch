using BaseArch.Application.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace BaseArch.Application.Registrations
{
    public static class AuthHandlingMiddlewareRegistration
    {
        public static void UserAuthHandlingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<AuthHandlingMiddleware>();
        }
    }
}

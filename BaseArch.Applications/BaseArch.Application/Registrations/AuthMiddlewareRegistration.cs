using BaseArch.Application.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace BaseArch.Application.Registrations
{
    public static class AuthMiddlewareRegistration
    {
        public static void UserAuthMiddleware(this WebApplication app)
        {
            app.UseMiddleware<AuthMiddleware>();
        }
    }
}

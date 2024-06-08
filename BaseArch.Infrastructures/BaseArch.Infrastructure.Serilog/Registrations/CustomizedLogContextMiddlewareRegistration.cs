using BaseArch.Infrastructure.Serilog.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace BaseArch.Infrastructure.Serilog.Registrations
{
    /// <summary>
    /// Extension methods for <see cref="WebApplication"/> to register middlewares
    /// </summary>
    public static class CustomizedLogContextMiddlewareRegistration
    {
        /// <summary>
        /// Use the <see cref="CustomizedLogContextMiddleware"/> to add customized properties to <see cref="LogContext"/>
        /// </summary>
        /// <param name="app"><see cref="WebApplication"/></param>
        public static void UseCustomizedSerilogLogContextMiddleware(this WebApplication app)
        {
            app.UseMiddleware<CustomizedLogContextMiddleware>();
        }
    }
}

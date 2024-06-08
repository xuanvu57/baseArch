using BaseArch.Presentation.RestApi.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace BaseArch.Presentation.RestApi.Registrations
{
    /// <summary>
    /// Extension methods for <see cref="WebApplication"/> to use <see cref="CorrelationIdMiddleware"/>
    /// </summary>
    public static class CorrelationIdMiddlewareRegistration
    {
        /// <summary>
        /// Use <see cref="CorrelationIdMiddleware"/>
        /// </summary>
        /// <param name="app"><see cref="WebApplication"/></param>
        public static void UseCorrelationIdMiddleware(this WebApplication app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}

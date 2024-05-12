using BaseArch.Presentation.RestApi.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace BaseArch.Presentation.RestApi.Extensions
{
    public static class HttpRequestResponseLoggingMiddlewareRegistration
    {
        public static void UseHttpRequestResponseLoggingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<HttpRequestResponseLoggingMiddleware>();
        }
    }
}

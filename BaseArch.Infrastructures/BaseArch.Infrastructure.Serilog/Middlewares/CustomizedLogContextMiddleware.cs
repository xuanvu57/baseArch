using BaseArch.Application.CorrelationId.Interfaces;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace BaseArch.Infrastructure.Serilog.Middlewares
{
    /// <summary>
    /// Customized serilog log context middleware to add customized properties to <see cref="LogContext"/>
    /// </summary>
    /// <param name="next"></param>
    public class CustomizedLogContextMiddleware(RequestDelegate next)
    {
        /// <summary>
        /// Invoke the handler
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/></param>
        /// <param name="correlationIdProvider"><see cref="ICorrelationIdProvider"/> to get colleration id</param>
        /// <returns><see cref="Task"/></returns>
        public Task Invoke(HttpContext context, ICorrelationIdProvider correlationIdProvider)
        {
            var correlationId = correlationIdProvider.Get();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                return next.Invoke(context);
            }
        }
    }
}

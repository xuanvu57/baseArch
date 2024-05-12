using BaseArch.Application.CorrelationId;
using BaseArch.Application.CorrelationId.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace BaseArch.Presentation.RestApi.Middlewares
{
    /// <summary>
    /// Correlation middleware
    /// </summary>
    /// <param name="next"><see cref="RequestDelegate"/></param>
    /// <param name="options"><see cref="CorrelationIdOptions"/></param>
    public class CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdOptions> options)
    {
        /// <summary>
        /// Invoke the middleware handler
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <param name="correlationIdProvider"><see cref="ICorrelationIdProvider"/></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext, ICorrelationIdProvider correlationIdProvider)
        {
            var currentCorrelationId = GetCorrelationIdFromRequestHeader(httpContext);
            if (string.IsNullOrEmpty(currentCorrelationId))
            {
                currentCorrelationId = GenerateCorrelationIdAndAppendRequestHeader(httpContext, correlationIdProvider);
            }

            correlationIdProvider.Set(currentCorrelationId);
            AddCorrelationIdToResponseHeader(httpContext, correlationIdProvider);

            await next(httpContext);
        }

        /// <summary>
        /// Get current correlation id from request header
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <returns>Current correlation id</returns>
        private string GetCorrelationIdFromRequestHeader(HttpContext httpContext)
        {
            var hasValue = httpContext.Request.Headers.TryGetValue(options.Value.RequestHeader, out var values);

            if (hasValue && !StringValues.IsNullOrEmpty(values))
            {
                return values.FirstOrDefault() ?? string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Generate new correlation id then append to current request header
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <param name="correlationIdProvider"><see cref="ICorrelationIdProvider"/></param>
        /// <returns>New correlation id</returns>
        private string GenerateCorrelationIdAndAppendRequestHeader(HttpContext httpContext, ICorrelationIdProvider correlationIdProvider)
        {
            var correlationId = correlationIdProvider.Generate();

            httpContext.Request.Headers.Append(options.Value.RequestHeader, new[] { correlationId });

            return correlationId;
        }

        /// <summary>
        /// Add correlation id to response header
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <param name="correlationIdProvider"><see cref="ICorrelationIdProvider"/></param>
        private void AddCorrelationIdToResponseHeader(HttpContext httpContext, ICorrelationIdProvider correlationIdProvider)
        {
            httpContext.Response.OnStarting(() =>
            {
                if (!httpContext.Response.Headers.ContainsKey(options.Value.ResponseHeader))
                {
                    httpContext.Response.Headers.Append(options.Value.ResponseHeader, new[] { correlationIdProvider.Get() });
                }
                return Task.CompletedTask;
            });
        }
    }
}

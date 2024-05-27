using BaseArch.Application.CorrelationId;
using BaseArch.Application.CorrelationId.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BaseArch.Infrastructure.DefaultHttpClient.DelegatingHandlers
{
    public class HttpClientCorrelationIdDelegatingHandler(IHttpContextAccessor accessor) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddCorrelationIdToRequestHeader(request);

            return await base.SendAsync(request, cancellationToken);
        }

        private void AddCorrelationIdToRequestHeader(HttpRequestMessage request)
        {
            if (accessor.HttpContext is null)
                return;

            var correlationIdProvider = accessor.HttpContext.RequestServices.GetRequiredService<ICorrelationIdProvider>();
            var options = accessor.HttpContext.RequestServices.GetRequiredService<IOptions<CorrelationIdOptions>>();

            var correlationId = correlationIdProvider.Get();

            if (!request.Headers.Contains(options.Value.RequestHeader))
            {
                request.Headers.Add(options.Value.RequestHeader, correlationId);
            }
        }
    }
}

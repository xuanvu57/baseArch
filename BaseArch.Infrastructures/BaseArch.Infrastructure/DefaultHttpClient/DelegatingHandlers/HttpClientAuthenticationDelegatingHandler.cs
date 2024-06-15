using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace BaseArch.Infrastructure.DefaultHttpClient.DelegatingHandlers
{
    public class HttpClientAuthenticationDelegatingHandler(IHttpContextAccessor accessor) : DelegatingHandler
    {
        private const string DefaultScheme = "Bearer";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddAuthorizationToRequestHeader(request);

            return await base.SendAsync(request, cancellationToken);
        }

        private void AddAuthorizationToRequestHeader(HttpRequestMessage request)
        {
            if (accessor.HttpContext is null)
                return;

            if (request.Headers.Authorization is null)
            {
                var authorizationValue = accessor.HttpContext.Request.Headers.Authorization.FirstOrDefault() ?? "";
                if (authorizationValue.StartsWith(DefaultScheme))
                {
                    var token = authorizationValue.Replace($"{DefaultScheme} ", "");
                    request.Headers.Authorization = new AuthenticationHeaderValue(DefaultScheme, token);
                }
            }
        }
    }
}

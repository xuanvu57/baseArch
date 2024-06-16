using BaseArch.Application.Identity.Interfaces;
using System.Net.Http.Headers;

namespace BaseArch.Infrastructure.DefaultHttpClient.DelegatingHandlers
{
    public class HttpClientAuthenticationDelegatingHandler(ITokenProvider tokenProvider) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddAuthorizationToRequestHeader(request);

            return await base.SendAsync(request, cancellationToken);
        }

        private void AddAuthorizationToRequestHeader(HttpRequestMessage request)
        {
            if (request.Headers.Authorization is not null)
                return;

            var token = tokenProvider.GetAccessToken();

            if (string.IsNullOrEmpty(token))
                return;

            request.Headers.Authorization = new AuthenticationHeaderValue(tokenProvider.DefaultScheme, token);
        }
    }
}

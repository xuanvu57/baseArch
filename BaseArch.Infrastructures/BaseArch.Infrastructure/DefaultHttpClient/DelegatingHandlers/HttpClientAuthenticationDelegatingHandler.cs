using BaseArch.Application.Identity.Interfaces;
using System.Net.Http.Headers;

namespace BaseArch.Infrastructure.DefaultHttpClient.DelegatingHandlers
{
    /// <summary>
    /// Delegating handler to add authenticated token into request
    /// </summary>
    /// <param name="tokenProvider"><see cref="ITokenProvider"/></param>
    public class HttpClientAuthenticationDelegatingHandler(ITokenProvider tokenProvider) : DelegatingHandler
    {
        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddAuthenticatedTokenToRequestHeader(request);

            return await base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Add authenticated token to request header
        /// </summary>
        /// <param name="request"><see cref="HttpRequestMessage"/></param>
        private void AddAuthenticatedTokenToRequestHeader(HttpRequestMessage request)
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

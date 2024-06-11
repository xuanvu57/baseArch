using BaseArch.Application.Identity.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.DefaultHttpClient.Registrations;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Web;

namespace BaseArch.Infrastructure.Identity.Sso.Google
{
    /// <summary>
    /// Google single sign-on provider
    /// </summary>
    /// <param name="options"><see cref="GoogleSsoOptions"/></param>
    /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/></param>
    [DIService(DIServiceLifetime.Scoped)]
    public class GoogleSsoProvider(IOptions<GoogleSsoOptions> options, IHttpClientFactory httpClientFactory) : ISsoProvider
    {
        //https://www.codeproject.com/Articles/5376012/Using-Google-OAuth-2-0-as-User-Sign-In-for-ASP-NET?fid=2004592&df=90&mpp=25&sort=Position&spc=Relaxed&prof=True&view=Normal&fr=6

        /// <inheritdoc/>
        public string Name { get; } = "Google";

        /// <inheritdoc/>
        public string GetLoginUrl()
        {
            var clientId = options.Value.ClientId;
            var redirectUri = HttpUtility.UrlEncode($"{options.Value.RedirectUrl}");

            return $@"{options.Value.GoogleLoginUrl}?" +
            $"access_type=offline" +
            $"&client_id={clientId}" +
            $"&redirect_uri={redirectUri}" +
            $"&response_type=code" +
            $"&scope=email profile" +
            $"&prompt=consent";
        }

        /// <inheritdoc/>
        public async Task<string> GetToken(string authorizationCode)
        {
            var data = new Dictionary<string, string>()
            {
                { "client_id", options.Value.ClientId },
                { "client_secret", options.Value.ClientSecret },
                { "code", authorizationCode },
                { "grant_type", "authorization_code" },
                { "redirect_uri", options.Value.RedirectUrl },
                { "access_type", "offline" }
            };
            using var content = new FormUrlEncodedContent(data);

            var httpClient = httpClientFactory.CreateClient(HttpClientRegistration.DefaultClientName);
            var response = await httpClient.PostAsync(options.Value.OAuthGoogleApiUrl, content);

            return await response.Content.ReadAsStringAsync();
        }

        /// <inheritdoc/>
        public async Task<string> RenewAccessToken(string refreshToken)
        {
            var data = new Dictionary<string, string>()
            {
                { "client_id", options.Value.ClientId },
                { "client_secret", options.Value.ClientSecret },
                { "refresh_token", refreshToken },
                { "grant_type", "refresh_token" },
            };
            using var content = new FormUrlEncodedContent(data);

            var httpClient = httpClientFactory.CreateClient(HttpClientRegistration.DefaultClientName);
            var response = await httpClient.PostAsync(options.Value.OAuthGoogleApiUrl, content);

            return await response.Content.ReadAsStringAsync();
        }

        /// <inheritdoc/>
        public async Task<string> GetUserInfo(string accessToken)
        {
            var httpClient = httpClientFactory.CreateClient(HttpClientRegistration.DefaultClientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync(options.Value.GoogleApiUrl);

            return await response.Content.ReadAsStringAsync();
        }
    }
}

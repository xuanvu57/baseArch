using BaseArch.Application.Identity.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.DefaultHttpClient.Registrations;
using BaseArch.Infrastructure.Identity.Sso.Facebook.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;

//https://developers.facebook.com/docs/facebook-login/guides/advanced/manual-flow/#login
namespace BaseArch.Infrastructure.Identity.Sso.Facebook
{
    /// <summary>
    /// Facebook single sign-on provider
    /// </summary>
    /// <param name="options"><see cref="FacebookSsoOptions"/></param>
    /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/></param>
    [DIService(DIServiceLifetime.Scoped)]
    public class FacebookSsoProvider(IOptions<FacebookSsoOptions> options, IHttpClientFactory httpClientFactory) : ISsoProvider
    {
        /// <inheritdoc/>
        public string Name { get; } = "Facebook";

        /// <inheritdoc/>
        public string GetLoginUrl()
        {
            var redirectUri = HttpUtility.UrlEncode($"{options.Value.RedirectUrl}");

            return $@"{options.Value.FacebookLoginUrl}?" +
            $"&client_id={options.Value.AppId}" +
            $"&redirect_uri={redirectUri}" +
            $"&auth_type=rerequest" +
            $"&state={options.Value.State}";
        }

        /// <inheritdoc/>
        public async Task<string> GetToken(string authorizationCode)
        {
            var data = new Dictionary<string, string>()
            {
                { "client_id", options.Value.AppId },
                { "client_secret", options.Value.AppSecret },
                { "code", authorizationCode },
                { "redirect_uri", options.Value.RedirectUrl }
            };
            using var content = new FormUrlEncodedContent(data);

            var httpClient = httpClientFactory.CreateClient(HttpClientRegistration.DefaultClientName);
            var response = await httpClient.PostAsync(options.Value.OAuthFacebookApiUrl, content);

            var jsonToken = await response.Content.ReadAsStringAsync();

            var facebokToken = JsonSerializer.Deserialize<FacebookSsoTokenModel>(jsonToken);

            if (facebokToken is null)
            {
                return jsonToken;
            }
            return await GetLongLivedToken(facebokToken.AccessToken);
        }

        /// <inheritdoc/>
        public async Task<string> GetUserInfo(string accessToken)
        {
            var userInfoUri = $"{options.Value.FacebookApiUrl}?fields={options.Value.Fields}";

            var httpClient = httpClientFactory.CreateClient(HttpClientRegistration.DefaultClientName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync(userInfoUri);

            return await response.Content.ReadAsStringAsync();
        }

        /// <inheritdoc/>
        public async Task<string> RenewAccessToken(string refreshToken)
        {
            return await GetLongLivedToken(refreshToken);
        }

        /// <summary>
        /// Get long lived access token
        /// </summary>
        /// <param name="token">Short lived access token</param>
        /// <returns>Long lived access token</returns>
        private async Task<string> GetLongLivedToken(string token)
        {
            var data = new Dictionary<string, string>()
            {
                { "client_id", options.Value.AppId },
                { "client_secret", options.Value.AppSecret },
                { "grant_type", "fb_exchange_token" },
                { "fb_exchange_token", token }
            };
            using var content = new FormUrlEncodedContent(data);

            var httpClient = httpClientFactory.CreateClient(HttpClientRegistration.DefaultClientName);
            var response = await httpClient.PostAsync(options.Value.OAuthFacebookApiUrl, content);

            return await response.Content.ReadAsStringAsync();
        }
    }
}

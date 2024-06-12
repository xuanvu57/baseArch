using Application.Identity.Services.Interfaces;
using BaseArch.Application.Identity.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.Identity.Sso.Google;
using BaseArch.Infrastructure.Identity.Sso.Google.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Web;

namespace Infrastructure.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    public class OAuthGoogleService(IEnumerable<ISsoProvider> ssoProviders, IOptions<GoogleSsoOptions> options, ILoginService loginService) : IOAuthGoogleService
    {
        private readonly ISsoProvider provider = ssoProviders.First(x => x.Name == "Google");

        public async Task<string> GetTokenAndCreateCallbackUrl(string authorizationCode)
        {
            var jsonToken = await provider.GetToken(authorizationCode);

            var googleToken = JsonSerializer.Deserialize<GoogleSsoTokenModel>(jsonToken);

            var callbackUrl = options.Value.CallbackUrl;
            if (googleToken is not null && googleToken.IsSuccess)
            {
                var jsonUserInfo = await provider.GetUserInfo(googleToken.AccessToken);

                var userInfo = JsonSerializer.Deserialize<GoogleSsoUserInfoModel>(jsonUserInfo);
                if (userInfo is not null)
                {
                    var query = HttpUtility.ParseQueryString(string.Empty);
                    query["ssoAccessToken"] = googleToken.AccessToken;
                    query["ssoRefreshToken"] = googleToken.RefreshToken;

                    var token = await loginService.Login(userInfo.Email);
                    if (token is not null)
                    {
                        query["accessToken"] = token.AccessToken;
                        query["refreshToken"] = token.RefreshToken;
                    }

                    callbackUrl = $"{callbackUrl}?{query}";
                }
            }

            return callbackUrl;
        }
    }
}

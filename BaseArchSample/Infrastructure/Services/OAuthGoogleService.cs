using Application.Identity.Services.Interfaces;
using BaseArch.Application.Identity.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.Identity.Models.GoogleSso;
using BaseArch.Infrastructure.Identity.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    public class OAuthGoogleService(IEnumerable<ISsoProvider> ssoProviders, IOptions<GoogleSsoOptions> options, ILoginService loginService) : IOAuthGoogleService
    {
        private readonly ISsoProvider googleSsoProvider = ssoProviders.First(x => x.Name == "Google");

        public async Task<string> GetTokenAndCreateCallbackUrl(string authorizationCode)
        {
            var jsonToken = await googleSsoProvider.GetToken(authorizationCode);

            var googleToken = JsonSerializer.Deserialize<GoogleSsoTokenModel>(jsonToken);

            var callbackUrl = options.Value.CallbackUrl;
            if (googleToken is not null && googleToken.IsSuccess)
            {
                var jsonUserInfo = await googleSsoProvider.GetUserInfo(googleToken.AccessToken);

                var userInfo = JsonSerializer.Deserialize<GoogleSsoUserInfoModel>(jsonUserInfo);
                if (userInfo is not null)
                {
                    var token = await loginService.Login(userInfo.Email);
                    if (token is not null)
                    {
                        callbackUrl = $"{callbackUrl}?accessToken={token.AccessToken}&refreshToken={token.RefreshToken}";
                        callbackUrl = $"{callbackUrl}&googleAccessToken={googleToken.AccessToken}&googleRefreshToken={googleToken.RefreshToken}";
                    }
                }
            }

            return callbackUrl;
        }
    }
}

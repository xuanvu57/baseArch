using Application.Identity.Services.Interfaces;
using BaseArch.Application.Identity.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.Identity.Sso.Facebook;
using BaseArch.Infrastructure.Identity.Sso.Facebook.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Web;

namespace Infrastructure.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    public class OAuthFacebookService(IEnumerable<ISsoProvider> ssoProviders, IOptions<FacebookSsoOptions> options, ILoginService loginService) : IOAuthFacebookService
    {

        public async Task<string> GetTokenAndCreateCallbackUrl(string code, string state, bool success)
        {
            var provider = ssoProviders.First(x => x.Name == "Facebook");
            var callbackUrl = options.Value.CallbackUrl;
            if (success && state == options.Value.State)
            {
                var jsonToken = await provider.GetToken(code);

                var facebokToken = JsonSerializer.Deserialize<FacebookSsoTokenModel>(jsonToken);

                if (facebokToken is not null)
                {
                    var jsonUserInfo = await provider.GetUserInfo(facebokToken.AccessToken);

                    var userInfo = JsonSerializer.Deserialize<FacebookSsoUserInfoModel>(jsonUserInfo);
                    if (userInfo is not null)
                    {
                        var query = HttpUtility.ParseQueryString(string.Empty);
                        query["ssoAccessToken"] = facebokToken.AccessToken;
                        query["ssoRefreshToken"] = facebokToken.AccessToken;

                        var token = await loginService.Login(userInfo.Email);
                        if (token is not null)
                        {
                            query["accessToken"] = token.AccessToken;
                            query["refreshToken"] = token.RefreshToken;
                        }

                        callbackUrl = $"{callbackUrl}?{query}";
                    }
                }
            }

            return callbackUrl;
        }
    }
}

using Application.Identity.Dtos;
using Application.Identity.Dtos.Responses;
using Application.Identity.Services.Interfaces;
using BaseArch.Application.Identity.Interfaces;
using BaseArch.Domain.DependencyInjection;

namespace Application.Identity.Services
{
    [DIService(DIServiceLifetime.Scoped)]
    public class RefreshTokenService(ITokenProvider tokenProvider) : IRefreshTokenService
    {
        public TokenResponse Refresh(RefreshTokenRequest request)
        {
            var tokens = tokenProvider.CreateAccessTokenFromRefreshToken(request.RefreshToken, request.AccessToken);

            if (!string.IsNullOrEmpty(tokens.newAccessToken) &&
                !string.IsNullOrEmpty(tokens.newRefreshToken))
            {
                return new TokenResponse()
                {
                    AccessToken = tokens.newAccessToken,
                    RefreshToken = tokens.newRefreshToken,
                };
            }

            return null;
        }
    }
}

using System.Security.Claims;

namespace BaseArch.Application.Identity.Interfaces
{
    public interface ITokenProvider
    {
        string CreateAccessToken(IEnumerable<Claim> claims);

        (string newAccessToken, string newRefreshToken) CreateAccessTokenFromRefreshToken(string refreshToken, string accessToken);

        string CreateRefreshToken(string nameIdentifierClaimValue);
    }
}

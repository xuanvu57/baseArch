using System.Security.Claims;

namespace BaseArch.Application.Identity.Interfaces
{
    /// <summary>
    /// Token provider
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Create access token from claim
        /// </summary>
        /// <param name="claims">List of <see cref="Claim"/></param>
        /// <returns>Access token</returns>
        string CreateAccessToken(IEnumerable<Claim> claims);

        /// <summary>
        /// Renew access token from refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <param name="accessToken">Old access token</param>
        /// <returns>New access token and new refresh token</returns>
        (string newAccessToken, string newRefreshToken) RenewAccessTokenFromRefreshToken(string refreshToken, string accessToken);

        /// <summary>
        /// Create refresh token, include the name identitifier
        /// </summary>
        /// <param name="nameIdentifierClaimValue">Name identifier</param>
        /// <returns>Refresh token</returns>
        string CreateRefreshToken(string nameIdentifierClaimValue);
    }
}

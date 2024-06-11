namespace BaseArch.Application.Identity.Interfaces
{
    /// <summary>
    /// Single sign-on provider
    /// </summary>
    public interface ISsoProvider
    {
        /// <summary>
        /// Provider name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get login url to identity server
        /// </summary>
        /// <returns>Url</returns>
        string GetLoginUrl();

        /// <summary>
        /// Get token from identity server
        /// </summary>
        /// <param name="authorizationCode">Authentication code from redirect url</param>
        /// <returns>Token information</returns>
        Task<string> GetToken(string authorizationCode);

        /// <summary>
        /// Renew access token from the refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>New token information</returns>
        Task<string> RenewAccessToken(string refreshToken);

        /// <summary>
        /// Get user information from identity server
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <returns>User information</returns>
        Task<string> GetUserInfo(string accessToken);
    }
}

namespace BaseArch.Infrastructure.Identity.Jwt
{
    /// <summary>
    /// Jwt options
    /// </summary>
    public sealed record JwtOptions
    {
        /// <summary>
        /// Issuer to validate the jwt token
        /// </summary>
        public required string ValidIssuer { get; init; }

        /// <summary>
        /// Audience to valide the jwt token
        /// </summary>
        public required string ValidAudience { get; init; }

        /// <summary>
        /// Secret key to generate and validate jwt token
        /// </summary>
        public required string SecrectKey { get; init; }

        /// <summary>
        /// Expired time in minute for access token
        /// </summary>
        public required int AccessTokenExpirationInMinute { get; init; }

        /// <summary>
        /// Expired time in minute for refresh token
        /// </summary>
        public required int RefreshTokenExpirationInMinute { get; init; }
    }
}

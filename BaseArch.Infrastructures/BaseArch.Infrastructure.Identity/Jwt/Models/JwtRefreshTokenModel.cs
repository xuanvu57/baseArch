namespace BaseArch.Infrastructure.Identity.Jwt.Models
{
    /// <summary>
    /// Jwt refresh token model before encryption
    /// </summary>
    internal sealed record JwtRefreshTokenModel
    {
        /// <summary>
        /// Token as a sign key
        /// </summary>
        public required string Token { get; init; }

        /// <summary>
        /// Name identifier to identify the user key
        /// </summary>
        public required string NameIdentifier { get; init; }

        /// <summary>
        /// Expired time
        /// </summary>
        public DateTimeOffset ExpiredAt { get; init; }
    }
}

namespace BaseArch.Infrastructure.Identity.Options
{
    public record JwtOptions
    {
        public required string ValidIssuer { get; init; }
        public required string ValidAudience { get; init; }
        public required string SecrectKey { get; init; }
        public required int AccessTokenExpirationInMinute { get; init; }
        public required int RefreshTokenExpirationInMinute { get; init; }
    }
}

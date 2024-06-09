namespace BaseArch.Infrastructure.Identity.Options
{
    public record GoogleSsoOptions
    {
        public bool Enable { get; init; }
        public required string RedirectUrl { get; init; }
        public required string CallbackUrl { get; init; }
        public required string ClientId { get; init; }
        public required string ClientSecret { get; init; }
        public required string GoogleLoginUrl { get; init; }
        public required string OAuthGoogleApiUrl { get; init; }
        public required string GoogleApiUrl { get; init; }
    }
}

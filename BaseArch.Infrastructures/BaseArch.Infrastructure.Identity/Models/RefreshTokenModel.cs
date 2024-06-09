namespace BaseArch.Infrastructure.Identity.Models
{
    internal record RefreshTokenModel
    {
        public required string Token { get; init; }
        public required string NameIdentifier { get; init; }
        public DateTimeOffset ExpiredAt { get; init; }
    }
}

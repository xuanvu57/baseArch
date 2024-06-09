namespace Application.Identity.Dtos.Requests
{
    public record RefreshTokenRequest
    {
        public required string RefreshToken { get; init; }
        public required string AccessToken { get; init; }
    }
}

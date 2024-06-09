namespace Application.Identity.Dtos
{
    public record RefreshTokenRequest
    {
        public string RefreshToken { get; init; }
        public string AccessToken { get; init; }
    }
}

namespace Application.Identity.Dtos.Responses
{
    public record TokenResponse
    {
        public required string AccessToken { get; init; }
        public required string RefreshToken { get; init; }
    }
}

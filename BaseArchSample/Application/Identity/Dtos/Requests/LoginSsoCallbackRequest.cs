namespace Application.Identity.Dtos.Requests
{
    public record LoginSsoCallbackRequest
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
        public string SsoAccessToken { get; init; } = string.Empty;
        public string SsoRefreshToken { get; init; } = string.Empty;
    }
}

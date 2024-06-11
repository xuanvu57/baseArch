namespace Application.Identity.Dtos.Requests
{
    public record LoginGoogleCallbackRequest
    {
        public string AccessToken { get; init; } = "";
        public string RefreshToken { get; init; } = "";
        public string GoogleAccessToken { get; init; } = "";
        public string GoogleRefreshToken { get; init; } = "";
    }
}

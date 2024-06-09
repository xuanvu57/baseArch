using System.Text.Json.Serialization;

namespace BaseArch.Infrastructure.Identity.Models.GoogleSso
{
    public record GoogleSsoTokenModel
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; } = "";

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; init; } = "";

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

        [JsonPropertyName("scope")]
        public string Scope { get; init; } = "";

        [JsonPropertyName("token_type")]
        public string TokenType { get; init; } = "";

        [JsonPropertyName("id_token")]
        public string IdToken { get; init; } = "";

        [JsonPropertyName("error")]
        public string Error { get; set; } = "";

        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; } = "";

        public bool IsSuccess => string.IsNullOrEmpty(Error);
    }
}

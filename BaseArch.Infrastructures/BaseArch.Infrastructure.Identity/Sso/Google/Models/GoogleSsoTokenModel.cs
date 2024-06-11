using System.Text.Json.Serialization;

namespace BaseArch.Infrastructure.Identity.Sso.Google.Models
{
    /// <summary>
    /// Token model from Google single sign-on
    /// </summary>
    public record GoogleSsoTokenModel
    {
        /// <summary>
        /// Access token
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; } = string.Empty;

        /// <summary>
        /// Refresh token
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; init; } = string.Empty;

        /// <summary>
        /// Expired time in second
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

        /// <summary>
        /// Scope
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; init; } = string.Empty;

        /// <summary>
        /// Type of token
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; init; } = string.Empty;

        /// <summary>
        /// Token id
        /// </summary>
        [JsonPropertyName("id_token")]
        public string IdToken { get; init; } = string.Empty;

        /// <summary>
        /// Error if there is problem
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Error description
        /// </summary>
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; } = string.Empty;

        /// <summary>
        /// Status for the token model
        /// </summary>
        public bool IsSuccess => string.IsNullOrEmpty(Error);
    }
}

using System.Text.Json.Serialization;

namespace BaseArch.Infrastructure.Identity.Models.GoogleSso
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
        public string AccessToken { get; init; } = "";

        /// <summary>
        /// Refresh token
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; init; } = "";

        /// <summary>
        /// Expired time in second
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

        /// <summary>
        /// Scope
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; init; } = "";

        /// <summary>
        /// Type of token
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; init; } = "";

        /// <summary>
        /// Token id
        /// </summary>
        [JsonPropertyName("id_token")]
        public string IdToken { get; init; } = "";

        /// <summary>
        /// Error if there is problem
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; } = "";

        /// <summary>
        /// Error description
        /// </summary>
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; set; } = "";

        /// <summary>
        /// Status for the token model
        /// </summary>
        public bool IsSuccess => string.IsNullOrEmpty(Error);
    }
}

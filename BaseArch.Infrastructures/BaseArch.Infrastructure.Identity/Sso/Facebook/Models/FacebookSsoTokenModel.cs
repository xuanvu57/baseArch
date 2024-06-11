using System.Text.Json.Serialization;

namespace BaseArch.Infrastructure.Identity.Sso.Facebook.Models
{
    /// <summary>
    /// Token model from Facebook single sign-on
    /// </summary>
    public record FacebookSsoTokenModel
    {
        /// <summary>
        /// Access token
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; } = string.Empty;

        /// <summary>
        /// Type of token
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; init; } = string.Empty;

        /// <summary>
        /// Expired time in second
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }
    }
}

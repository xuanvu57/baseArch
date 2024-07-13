using System.Text.Json.Serialization;

namespace BaseArch.Infrastructure.Identity.Sso.Google.Models
{
    /// <summary>
    /// User information model from Google single sign-on
    /// </summary>
    public sealed record GoogleSsoUserInfoModel
    {
        /// <summary>
        /// User id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; init; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; init; } = string.Empty;

        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Url to user's avatar
        /// </summary>
        [JsonPropertyName("picture")]
        public string Picutre { get; init; } = string.Empty;

        /// <summary>
        /// Given name
        /// </summary>
        [JsonPropertyName("given_name")]
        public string GivenName { get; init; } = string.Empty;

        /// <summary>
        /// Family name
        /// </summary>
        [JsonPropertyName("family_name")]
        public string FamilyName { get; init; } = string.Empty;

        /// <summary>
        /// <see cref="GoogleSsoUserInfoErrorModel"/>
        /// </summary>
        public GoogleSsoUserInfoErrorModel? Error { get; init; }

        /// <summary>
        /// Status for the user information model
        /// </summary>
        public bool IsSuccess => Error == null;
    }
}

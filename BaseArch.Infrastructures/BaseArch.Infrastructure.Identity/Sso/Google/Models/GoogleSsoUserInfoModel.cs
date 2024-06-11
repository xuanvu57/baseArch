using System.Text.Json.Serialization;

namespace BaseArch.Infrastructure.Identity.Sso.Google.Models
{
    /// <summary>
    /// User information model from Google single sign-on
    /// </summary>
    public partial record GoogleSsoUserInfoModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; init; } = "";

        /// <summary>
        /// Name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; init; } = "";

        /// <summary>
        /// Url to user's avatar
        /// </summary>
        [JsonPropertyName("picture")]
        public string Picutre { get; init; } = "";

        /// <summary>
        /// Given name
        /// </summary>
        [JsonPropertyName("given_name")]
        public string GivenName { get; init; } = "";

        /// <summary>
        /// Family name
        /// </summary>
        [JsonPropertyName("family_name")]
        public string FamilyName { get; init; } = "";

        /// <summary>
        /// <see cref="GoogleSsoUserInfoModel"/>
        /// </summary>
        public GoogleSsoUserInfoErrorModel? Error { get; init; }

        /// <summary>
        /// Status for the user information model
        /// </summary>
        public bool IsSuccess => Error == null;
    }
}

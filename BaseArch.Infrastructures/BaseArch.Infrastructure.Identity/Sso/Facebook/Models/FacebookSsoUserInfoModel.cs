using System.Text.Json.Serialization;

namespace BaseArch.Infrastructure.Identity.Sso.Facebook.Models
{
    public record FacebookSsoUserInfoModel
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
        public FacebookSsoUserInfoPictureModel? Picutre { get; init; }

        /// <summary>
        /// First name
        /// </summary>
        [JsonPropertyName("first_name")]
        public string FirstName { get; init; } = string.Empty;

        /// <summary>
        /// Last name
        /// </summary>
        [JsonPropertyName("last_name")]
        public string LastName { get; init; } = string.Empty;
    }
}

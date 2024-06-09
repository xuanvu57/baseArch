using System.Text.Json.Serialization;

namespace BaseArch.Infrastructure.Identity.Models.GoogleSso
{
    public partial record GoogleSsoUserInfoModel
    {
        [JsonPropertyName("email")]
        public string Email { get; init; } = "";

        [JsonPropertyName("name")]
        public string Name { get; init; } = "";

        [JsonPropertyName("picture")]
        public string Picutre { get; init; } = "";

        [JsonPropertyName("given_name")]
        public string GivenName { get; init; } = "";

        [JsonPropertyName("family_name")]
        public string FamilyName { get; init; } = "";

        public GoogleSsoUserInfoErrorModel? Error { get; init; }

        public bool IsSuccess => Error == null;
    }
}

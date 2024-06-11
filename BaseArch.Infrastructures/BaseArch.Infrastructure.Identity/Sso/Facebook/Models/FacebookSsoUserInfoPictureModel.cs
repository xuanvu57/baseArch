using System.Text.Json.Serialization;

namespace BaseArch.Infrastructure.Identity.Sso.Facebook.Models
{
    public record FacebookSsoUserInfoPictureModel
    {
        [JsonPropertyName("data")]
        public FacebookSsoUserInfoPictureDataModel? Data { get; init; }
    }
}

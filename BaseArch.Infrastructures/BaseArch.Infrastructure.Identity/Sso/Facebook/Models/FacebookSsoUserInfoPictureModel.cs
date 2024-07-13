using System.Text.Json.Serialization;

namespace BaseArch.Infrastructure.Identity.Sso.Facebook.Models
{
    public sealed record FacebookSsoUserInfoPictureModel
    {
        [JsonPropertyName("data")]
        public FacebookSsoUserInfoPictureDataModel? Data { get; init; }
    }
}

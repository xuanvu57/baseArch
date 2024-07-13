using System.Text.Json.Serialization;

namespace BaseArch.Infrastructure.Identity.Sso.Facebook.Models
{
    public sealed record FacebookSsoUserInfoPictureDataModel
    {
        [JsonPropertyName("height")]
        public int Height { get; init; }

        [JsonPropertyName("width")]
        public int Width { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; } = string.Empty;
    }
}

using System.Text.Json;

namespace BaseArch.Infrastructure.DefaultHttpClient
{
    internal static class DefaultJsonSerializerOptions
    {
        static public JsonSerializerOptions JsonSerializerOptions
        {
            get
            {
                return new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
            }
        }
    }
}

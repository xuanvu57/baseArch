using System.Text.Json;

namespace BaseArch.Infrastructure.DefaultHttpClient
{
    /// <summary>
    /// Default json serializer options
    /// </summary>
    internal static class DefaultJsonSerializerOptions
    {
        /// <see cref="JsonDocumentOptions"/>
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

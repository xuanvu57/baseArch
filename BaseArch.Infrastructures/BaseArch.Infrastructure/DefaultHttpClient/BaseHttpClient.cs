using BaseArch.Infrastructure.DefaultHttpClient.Registrations;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BaseArch.Infrastructure.DefaultHttpClient
{
    public abstract class BaseHttpClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        protected HttpClient CreateHttpClientFromConfigKey(string uriConfigKey)
        {
            var uri = configuration[uriConfigKey] ?? "";

            return CreateHttpClientFromUri(uri);
        }

        protected HttpClient CreateHttpClientFromUri(string uri)
        {
            var httpClient = httpClientFactory.CreateClient(HttpClientRegistration.DefaultClientName);
            httpClient.BaseAddress = new Uri(uri);

            return httpClient;
        }

        protected static string Serialize<T>(T TObject)
        {
            return JsonSerializer.Serialize(TObject);
        }

        protected static T? Deserialize<T>(string content)
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var TObject = JsonSerializer.Deserialize<T>(content, jsonSerializerOptions);

            return TObject;
        }
    }
}

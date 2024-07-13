using BaseArch.Infrastructure.DefaultHttpClient.Registrations;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace BaseArch.Infrastructure.DefaultHttpClient
{
    /// <summary>
    /// Base Http client
    /// </summary>
    /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    public abstract class BaseHttpClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        /// <summary>
        /// Create HttpClient instance with base address from key in configuration
        /// </summary>
        /// <param name="uriConfigKey">Key for Uri from configure file</param>
        /// <returns><see cref="HttpClient"/></returns>
        protected HttpClient CreateHttpClientFromConfigKey(string uriConfigKey)
        {
            var uri = configuration[uriConfigKey] ?? "";

            return CreateHttpClientFromUri(uri);
        }

        /// <summary>
        /// Create HttpClient instance with base address from specific Uri
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <returns><see cref="HttpClient"/></returns>
        protected HttpClient CreateHttpClientFromUri(string uri)
        {
            var httpClient = httpClientFactory.CreateClient(HttpClientRegistration.DefaultClientName);
            httpClient.BaseAddress = new Uri(uri);

            return httpClient;
        }

        /// <summary>
        /// Serialize the object to json
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="TObject">The object to serialize</param>
        /// <returns>Json string for the object</returns>
        protected static string Serialize<T>(T TObject)
        {
            return JsonSerializer.Serialize(TObject);
        }

        /// <summary>
        /// Deserialize the json to object
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="content">Json string</param>
        /// <returns>Type of object</returns>
        protected static T? Deserialize<T>(string content)
        {
            var TObject = JsonSerializer.Deserialize<T>(content, DefaultJsonSerializerOptions.JsonSerializerOptions);

            return TObject;
        }
    }
}

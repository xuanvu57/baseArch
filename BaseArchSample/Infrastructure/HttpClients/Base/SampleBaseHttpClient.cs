using BaseArch.Infrastructure.DefaultHttpClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.HttpClients.Base
{
    public abstract class SampleBaseHttpClient(IHttpClientFactory httpClientFactory, IConfiguration configuration) : BaseHttpClient(httpClientFactory, configuration)
    {
        private const string appConfigKey = "Services:BaseArchSample:Url";

        protected HttpClient CreateHttpClientFromConfigKey()
        {
            return CreateHttpClientFromConfigKey(appConfigKey);
        }
    }
}

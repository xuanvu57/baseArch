using Application.User.Dtos;
using Application.User.ExternalServices.Interfaces;
using BaseArch.Domain.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.HttpClients
{
    [DIService(DIServiceLifetime.Scoped)]
    public class GreetingClientOther(ILogger<GreetingClientOther> logger, IConfiguration configuration, IHttpClientFactory httpClientFactory) :
        SampleBaseHttpClient(httpClientFactory, configuration), IGreetingClientOther
    {
        public async Task<string> TryToSayHello(string fullName)
        {
            var httpClient = CreateHttpClientFromConfigKey();

            var response = await httpClient.GetAsync("/api/v1/Users/GetAllUsers");

            var responseContent = await response.Content.ReadAsStringAsync();

            var users = Deserialize<IEnumerable<UserInfo>>(responseContent);

            var user = users?.FirstOrDefault(u => u.FullName == fullName);

            logger.LogInformation($"[HttpClient] ApiService server response with: {user}");

            return await Task.FromResult($"{user?.FullName}");
        }
    }
}

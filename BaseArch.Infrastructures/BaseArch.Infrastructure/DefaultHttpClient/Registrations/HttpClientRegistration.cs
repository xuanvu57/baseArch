using BaseArch.Infrastructure.DefaultHttpClient.DelegatingHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.DefaultHttpClient.Registrations
{
    public static class HttpClientRegistration
    {
        public const string DefaultClientName = "HttpClient";

        public static void AddDefaultHttpClient(this IServiceCollection services)
        {
            AddDefaultHttpClient(services, [], []);
        }

        public static void AddDefaultHttpClient(this IServiceCollection services, string[] clientNames, Type[] delegatingHandlerTypes)
        {
            services.AddTransient<HttpClientLoggingDelegatingHandler>();
            services.AddTransient<HttpClientCorrelationIdDelegatingHandler>();
            foreach (var type in delegatingHandlerTypes)
            {
                services.AddTransient(type);
            }

            string[] extendClientNames = [DefaultClientName, .. clientNames];
            foreach (var clientName in extendClientNames)
            {
                var httpClientBuilder = services
                    .AddHttpClient(clientName, client =>
                    {

                    });

                httpClientBuilder.AddHttpMessageHandler<HttpClientCorrelationIdDelegatingHandler>();
                httpClientBuilder.AddHttpMessageHandler<HttpClientLoggingDelegatingHandler>();

                foreach (var type in delegatingHandlerTypes)
                {
                    httpClientBuilder.AddHttpMessageHandler(() => (DelegatingHandler)ActivatorUtilities.CreateInstance(services.BuildServiceProvider(), type));
                }
            }

            services.AddHttpContextAccessor();
        }
    }
}

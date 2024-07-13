using BaseArch.Infrastructure.DefaultHttpClient.DelegatingHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.DefaultHttpClient.Registrations
{
    /// <summary>
    /// Extension to register Http client
    /// </summary>
    public static class HttpClientRegistration
    {
        /// <summary>
        /// Default client name
        /// </summary>
        public const string DefaultClientName = "HttpClient";

        /// <summary>
        /// Add Http client with default configuration
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void AddDefaultHttpClient(this IServiceCollection services)
        {
            AddDefaultHttpClient(services, [], []);
        }

        /// <summary>
        /// Add Http client with customized client names and delegating handlers
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="clientNames">List of client names</param>
        /// <param name="delegatingHandlerTypes">List of delegating handlers</param>
        public static void AddDefaultHttpClient(this IServiceCollection services, string[] clientNames, Type[] delegatingHandlerTypes)
        {
            services.AddTransient<HttpClientLoggingDelegatingHandler>();
            services.AddTransient<HttpClientCorrelationIdDelegatingHandler>();
            services.AddTransient<HttpClientAuthenticationDelegatingHandler>();
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
                httpClientBuilder.AddHttpMessageHandler<HttpClientAuthenticationDelegatingHandler>();
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

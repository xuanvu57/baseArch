using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Presentation.RestApi.Extensions
{
    /// <summary>
    /// Extension methods to register common service for BaseArch.Presentation.RestApi
    /// </summary>
    public static class RestApiRegistration
    {
        /// <summary>
        /// Add necessary services for Rest api
        /// </summary>
        /// <param name="services"></param>
        public static void AddServicesForRestApi(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }
    }
}

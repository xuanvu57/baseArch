using BaseArch.Application.CorrelationId;
using BaseArch.Application.CorrelationId.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Application.Registrations
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to register correlation id
    /// </summary>
    public static class CorrelationIdRegistration
    {
        /// <summary>
        /// Add necessary services for correlation id with default option
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void AddCorrelationIdServices<TCorrelationIdProvider>(this IServiceCollection services) where TCorrelationIdProvider : class, ICorrelationIdProvider
        {
            services.AddCorrelationIdServices<TCorrelationIdProvider>(opt => { });
        }

        /// <summary>
        /// Add necessary services for correlation id with options
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="options"><see cref="CorrelationIdOptions"/></param>
        public static void AddCorrelationIdServices<TCorrelationIdProvider>(this IServiceCollection services, Action<CorrelationIdOptions> options) where TCorrelationIdProvider : class, ICorrelationIdProvider
        {
            services.AddOptions<CorrelationIdOptions>()
                .Configure(options)
                .Validate(options =>
                {
                    if (string.IsNullOrEmpty(options.RequestHeader) || string.IsNullOrEmpty(options.ResponseHeader))
                        return false;

                    return true;
                }, "CorrelationIdOptions is invalid");

            services.AddScoped<ICorrelationIdProvider, TCorrelationIdProvider>();
        }
    }
}

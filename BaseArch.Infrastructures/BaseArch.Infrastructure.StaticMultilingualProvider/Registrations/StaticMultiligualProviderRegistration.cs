using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.StaticMultilingualProvider.Registrations
{
    /// <summary>
    /// Extension methods to register localization services
    /// </summary>
    public static class StaticMultiligualProviderRegistration
    {
        /// <summary>
        /// Add services for localization with IStringLocalized
        /// It require to add RequestLocalizationMiddleware by UseRequestLocalization() 
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="supportedCultures">Array of supported cultures (language-region)</param>
        public static void AddStaticMultilingualProviders(this IServiceCollection services, string[] supportedCultures)
        {
            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture(supportedCultures[0])
                         .AddSupportedCultures(supportedCultures)
                         .AddSupportedUICultures(supportedCultures);
            });
        }

        /// <summary>
        /// Register Request localization to use <see cref="IStringLocalizer"/>
        /// </summary>
        /// <param name="app"></param>
        public static void UseStaticMultilingualProviders(this WebApplication app)
        {
            app.UseRequestLocalization();
        }
    }
}

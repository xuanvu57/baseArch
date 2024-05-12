using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Presentation.RestApi.Extensions
{
    /// <summary>
    /// The extension methods to register Api versioning
    /// </summary>
    public static class ApiVersioningRegistration
    {
        /// <summary>
        /// Define and add Rest Api versioning for controllers
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="defaultMajorVersion">Default major version if there is no definition in controllers</param>
        /// <param name="defaultMinorVersion">Default minor version if there is no definition in controllers</param>
        public static void AddRestApiVersioning(this IServiceCollection services, int defaultMajorVersion, int? defaultMinorVersion = null)
        {
            services
                .AddApiVersioning(config =>
                {
                    config.DefaultApiVersion = new Asp.Versioning.ApiVersion(defaultMajorVersion, defaultMinorVersion);
                    config.AssumeDefaultVersionWhenUnspecified = true;
                    config.ReportApiVersions = true;
                })
                .AddApiExplorer(x =>
                {
                    x.GroupNameFormat = "'v'VVV";
                    x.SubstituteApiVersionInUrl = true;
                });
        }
    }
}

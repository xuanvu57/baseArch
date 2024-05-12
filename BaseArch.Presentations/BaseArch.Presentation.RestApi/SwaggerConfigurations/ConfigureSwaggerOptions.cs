using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BaseArch.Presentation.RestApi.SwaggerConfigurations
{
    /// <summary>
    /// Swagger options
    /// </summary>
    /// <param name="provider"><see cref="IApiVersionDescriptionProvider"/></param>
    public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
    {
        /// <summary>
        /// Configure the version selections
        /// </summary>
        /// <param name="options"></param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        /// <summary>
        /// Create Api information
        /// </summary>
        /// <param name="apiDescription"><see cref="ApiVersionDescription"/></param>
        /// <returns><see cref="OpenApiInfo"/></returns>
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription apiDescription)
        {
            string description =
                $"""
                The API Document with Swagger. {(apiDescription.IsDeprecated ? "This API version has been deprecated." : string.Empty)}
                """;
            OpenApiInfo info = new OpenApiInfo()
            {
                Title = "xxx",
                Version = apiDescription.ApiVersion.ToString(),
                Description = description
            };

            return info;
        }
    }
}

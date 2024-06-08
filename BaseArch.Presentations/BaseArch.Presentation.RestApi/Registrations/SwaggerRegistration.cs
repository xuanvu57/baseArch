using Asp.Versioning.ApiExplorer;
using BaseArch.Presentation.RestApi.SwaggerConfigurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BaseArch.Presentation.RestApi.Registrations
{
    /// <summary>
    /// Extension methods to register swagger
    /// </summary>
    public static class SwaggerRegistration
    {
        /// <summary>
        /// Add customized swagger configuration with Api versioning
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(option =>
            {
                option.OperationFilter<CustomizedOperationFilter>();
                option.EnableAnnotations();
            });
        }

        /// <summary>
        /// Use swagger middleware and UI
        /// </summary>
        /// <param name="app"><see cref="WebApplication"/></param>
        public static void UseSwaggerMiddleware(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                if (provider is not null)
                {
                    foreach (var groupName in provider.ApiVersionDescriptions.Select(d => d.GroupName))
                    {
                        options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant());
                    }
                }
            });
        }
    }
}

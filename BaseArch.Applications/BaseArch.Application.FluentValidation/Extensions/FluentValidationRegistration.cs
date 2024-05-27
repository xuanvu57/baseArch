using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BaseArch.Application.FluentValidation.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to register FluentValidation
    /// </summary>
    public static class FluentValidationRegistration
    {
        /// <summary>
        /// Add validators that inherit from <see cref="AbstractValidator{T}"/> automatically
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void AddFluentValidators(this IServiceCollection services)
        {
            var assemblies = GetAssembliesHasAbstractValidator();

            services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblies(assemblies);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        /// <summary>
        /// Scan to find all assemblies has <see cref="AbstractValidator{T}"/>
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Assembly> GetAssembliesHasAbstractValidator()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.ExportedTypes.Any(
                    type => type.BaseType != null
                    && type.BaseType.IsGenericType
                    && type.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>)));

            return assemblies;
        }
    }
}

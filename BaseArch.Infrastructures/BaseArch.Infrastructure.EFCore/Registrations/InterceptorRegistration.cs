using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.EFCore.Registrations
{
    /// <summary>
    /// Extension to register interceptors automatically
    /// </summary>
    public static class InterceptorRegistration
    {
        private static List<Type> _interceptorTypes = [];

        /// <summary>
        /// Add all interceptors as singleton services
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void AddEFInterceptor(this IServiceCollection services)
        {
            var types = GetAllInterceptorTypes();

            foreach (var type in types)
            {
                services.AddSingleton(type);
            }
        }

        /// <summary>
        /// Get all interceptor services
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
        /// <returns>List of <see cref="IInterceptor"/></returns>
        public static IEnumerable<IInterceptor> GetEFInterceptors(this IServiceProvider serviceProvider)
        {
            var types = GetAllInterceptorTypes();

            return types.Select(type => (IInterceptor)serviceProvider.GetRequiredService(type));
        }

        /// <summary>
        /// Scan and get all interceptors from assemblies
        /// </summary>
        /// <returns></returns>
        private static List<Type> GetAllInterceptorTypes()
        {
            if (_interceptorTypes.Count == 0)
            {
                _interceptorTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(type => type.IsAssignableTo(typeof(IInterceptor)) && type.IsClass && !type.IsAbstract)
                    .ToList();
            }

            return _interceptorTypes;
        }
    }
}

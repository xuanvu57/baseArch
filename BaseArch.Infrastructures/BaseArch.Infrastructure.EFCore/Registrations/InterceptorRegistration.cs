using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.EFCore.Registrations
{
    public static class InterceptorRegistration
    {
        private static List<Type> interceptorTypes = null;

        public static void RegisterEFInterceptor(this IServiceCollection services)
        {
            var types = GetAllInterceptorTypes();

            foreach (var type in types)
            {
                services.AddSingleton(type);
            }
        }

        public static IEnumerable<IInterceptor> GetEFInterceptors(this IServiceProvider serviceProvider)
        {
            var types = GetAllInterceptorTypes();

            return types.Select(type => (IInterceptor)serviceProvider.GetRequiredService(type));
        }

        private static List<Type> GetAllInterceptorTypes()
        {
            interceptorTypes ??= AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsAssignableTo(typeof(IInterceptor)) && type.IsClass && !type.IsAbstract)
                .ToList();

            return interceptorTypes;
        }
    }
}

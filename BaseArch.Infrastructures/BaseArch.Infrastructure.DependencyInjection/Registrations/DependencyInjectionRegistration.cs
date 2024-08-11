using BaseArch.Application.ModuleRegistrations.Interfaces;
using BaseArch.Domain.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace BaseArch.Infrastructure.DependencyInjection.Registrations
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to register Dependency Injection automatically
    /// </summary>
    public static class DependencyInjectionRegistration
    {
        /// <summary>
        /// Scan and register dependency injections automatically
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void RegisterDependencyInjections(this IServiceCollection services)
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger(nameof(DependencyInjectionRegistration));
            logger.LogInformation("Load assemblies....");
            EnsureToLoadAllAssemblies();

            logger.LogInformation("Register services....");
            services.RegisterAdditionalModuleRegistration(logger);
            services.RegisterDIServices();
        }

        /// <summary>
        /// Scan and register the services or components with <see cref="DIServiceAttribute"/> with Scrutor
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        private static void RegisterDIServices(this IServiceCollection services)
        {
            var diServiceTypes = GetDIServiceTypes();

            services.Scan(scrutor =>
                scrutor.FromTypes(diServiceTypes)
                .AddClasses(c => c.Where(type => type.GetCustomAttribute<DIServiceAttribute>()?.Lifetime == DIServiceLifetime.Singleton))
                .AsImplementedInterfaces()
                .WithSingletonLifetime()

                .AddClasses(c => c.Where(type => type.GetCustomAttribute<DIServiceAttribute>()?.Lifetime == DIServiceLifetime.Scoped))
                .AsImplementedInterfaces()
                .WithScopedLifetime()

                .AddClasses(c => c.Where(type => type.GetCustomAttribute<DIServiceAttribute>()?.Lifetime == DIServiceLifetime.Transient))
                .AsImplementedInterfaces()
                .WithTransientLifetime()
            );
        }

        /// <summary>
        /// Scan and register the 3rd libraries with <see cref="IModuleRegistration"/> interfaces
        /// </summary>=
        /// <param name="services"><see cref="IServiceCollection"/></param>
        private static void RegisterAdditionalModuleRegistration(this IServiceCollection services, ILogger logger)
        {
            var additionalDITypes = GetAdditionalModuleRegistrationTypes();

            additionalDITypes.ForEach(type =>
            {
                var instance = ActivatorUtilities.CreateInstance(services.BuildServiceProvider(), type);
                if (instance is not null)
                {
                    logger.LogInformation("Register dependencies for {TypeName}....", type.FullName);
                    var diInstance = (IModuleRegistration)instance;
                    diInstance.Register(services);
                }
            });
        }

        /// <summary>
        /// Get all types of class with <see cref="DIServiceAttribute"/> attribute
        /// </summary>
        /// <returns>List of Type</returns>
        private static List<Type> GetDIServiceTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && type.CustomAttributes.Any(a => a.AttributeType == typeof(DIServiceAttribute)))
                .ToList();

            return types;
        }

        /// <summary>
        /// Get all types of class that implements from <see cref="IModuleRegistration"/>
        /// </summary>
        /// <returns>List of Type</returns>
        private static List<Type> GetAdditionalModuleRegistrationTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsAssignableTo(typeof(IModuleRegistration)) && type.IsClass && !type.IsAbstract)
                .ToList();

            return types;
        }

        /// <summary>
        /// Ensure to load all assemblies to memory
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        private static void EnsureToLoadAllAssemblies()
        {
            var loadedAssemblyFullNames = AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.FullName);
            var dllFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.TopDirectoryOnly);
            var unloadedDllFiles = dllFiles.Where(dllFile => !loadedAssemblyFullNames.Contains(AssemblyName.GetAssemblyName(dllFile).FullName)).ToList();
            unloadedDllFiles.ForEach(f =>
            {
                Assembly.Load(File.ReadAllBytes(f));
            });
        }
    }
}

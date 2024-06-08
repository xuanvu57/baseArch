using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.DependencyInjection.Interfaces;
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

            services.RegisterAdditionalDependencyInjections(logger);
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
                scrutor.FromAssembliesOf(diServiceTypes)
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
        /// Scan and register the 3rd libraries with <see cref="IDependencyInjection"/> interfaces
        /// </summary>=
        /// <param name="services"><see cref="IServiceCollection"/></param>
        private static void RegisterAdditionalDependencyInjections(this IServiceCollection services, ILogger logger)
        {
            var additionalDITypes = GetAdditionalDependencyInjectionTypes();

            additionalDITypes.ForEach(type =>
            {
                var instance = ActivatorUtilities.CreateInstance(services.BuildServiceProvider(), type);
                if (instance is not null)
                {
                    logger.LogInformation("Add dependency injection for {TypeName}....", type.FullName);
                    var diInstance = (IDependencyInjection)instance;
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
                .SelectMany(assembly => assembly.ExportedTypes)
                .Where(type => type.IsClass && type.CustomAttributes.Any(a => a.AttributeType == typeof(DIServiceAttribute)))
                .ToList();

            return types;
        }

        /// <summary>
        /// Get all types of class that implements from <see cref="IDependencyInjection"/>
        /// </summary>
        /// <returns>List of Type</returns>
        private static List<Type> GetAdditionalDependencyInjectionTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.ExportedTypes)
                .Where(type => type.IsAssignableTo(typeof(IDependencyInjection)) && type.IsClass && !type.IsAbstract)
                .ToList();

            return types;
        }

        /// <summary>
        /// Ensure to load all assemblies to memory
        /// </summary>
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

using BaseArch.Infrastructure.gRPC.Attributes;
using BaseArch.Infrastructure.gRPC.Interceptors;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.gRPC.Registrations
{
    /// <summary>
    /// Extension methods to register gRPC service
    /// </summary>
    public static class GrpcRegistration
    {
        /// <summary>
        /// Register all gRPC service with AddGrpc() method
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public static void AddGrpcServices(this IServiceCollection services)
        {
            AddGrpcServices(services, option =>
            {
                option.Interceptors.Add<GrpcServiceLoggingInterceptor>();
            });
        }

        /// <summary>
        /// Register all gRPC service with AddGrpc() method
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="options"><see cref="GrpcServiceOptions"/></param>
        public static void AddGrpcServices(this IServiceCollection services, Action<GrpcServiceOptions> options)
        {
            services.AddGrpc(options);
        }

        /// <summary>
        /// Auto scan and map the route for grpc Requests
        /// </summary>
        /// <param name="app"><see cref="WebApplication"/></param>
        public static void AutoMapGprcServices(this WebApplication app)
        {
            var method = typeof(GrpcEndpointRouteBuilderExtensions).GetMethod("MapGrpcService");

            var grpcBaseServices = GetGrpcServiceTypes();
            foreach (var service in grpcBaseServices)
            {
                method!.MakeGenericMethod(service).Invoke(null, [app]);
            }
        }

        /// <summary>
        /// Find all gRPC services from all assemblies
        /// </summary>
        /// <returns>List of gRPC service types</returns>
        private static List<System.Type> GetGrpcServiceTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && type.CustomAttributes.Any(a => a.AttributeType == typeof(GrpcServiceAttribute)))
                .ToList();

            return types;
        }
    }
}

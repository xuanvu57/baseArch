using BaseArch.Infrastructure.gRPC.Attributes;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BaseArch.Infrastructure.gRPC.Extensions
{
    public static class GrpcRegistration
    {
        public static void AddGrpcServices(this IServiceCollection services, Action<GrpcServiceOptions> options)
        {
            //https://github.com/grpc/grpc/blob/master/src/csharp/BUILD-INTEGRATION.md
            services.AddGrpc(options);
        }

        public static void AutoMapGprcServices(this WebApplication app)
        {
            MapGrpcServicesFromAssemblies(app, AppDomain.CurrentDomain.GetAssemblies());
        }

        private static void MapGrpcServicesFromAssemblies(IApplicationBuilder applicationBuilder, params Assembly[] assemblies)
        {
            if (assemblies.Length > 0)
            {
                var method = typeof(GrpcEndpointRouteBuilderExtensions).GetMethod("MapGrpcService");

                var grpcBaseServices = GetGrpcServiceTypes();

                foreach (var service in grpcBaseServices)
                {
                    method!.MakeGenericMethod(service).Invoke(null, [applicationBuilder]);
                }
            }
        }

        private static List<Type> GetGrpcServiceTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && type.CustomAttributes.Any(a => a.AttributeType == typeof(GrpcServiceAttribute)))
                .ToList();

            return types;
        }
    }
}

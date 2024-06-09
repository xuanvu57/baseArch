using BaseArch.Infrastructure.gRPC.Interceptors;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.gRPC
{
    public abstract class BaseGrpcClient(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        protected GrpcChannel CreateChannelFromConfigureKey(string uriConfigKey)
        {
            var uri = configuration[uriConfigKey] ?? "";

            return CreateChannelFromUri(uri);
        }

        protected GrpcChannel CreateChannelFromUri(string uri)
        {
            var uriAddress = new Uri(uri);
            var httpHandler = new HttpClientHandler();
            if (uriAddress.Scheme == "http")
            {
                httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }
            var channel = GrpcChannel.ForAddress(uriAddress, new GrpcChannelOptions { HttpHandler = httpHandler });

            return channel;
        }

        protected TClient CreateGrpcClient<TClient>(GrpcChannel channel, params Interceptor[] clientInterceptors) where TClient : class
        {
            var grpcClientLoggingInterceptor = ActivatorUtilities.CreateInstance(serviceProvider, typeof(GrpcClientLoggingInterceptor));
            var grpcClientCorrelationIdInterceptor = ActivatorUtilities.CreateInstance(serviceProvider, typeof(GrpcClientCorrelationIdInterceptor));

            var invoker = channel
                .Intercept((Interceptor)grpcClientCorrelationIdInterceptor)
                .Intercept((Interceptor)grpcClientLoggingInterceptor);

            if (clientInterceptors.Length > 0)
            {
                invoker.Intercept(clientInterceptors);
            }

            var client = ActivatorUtilities.CreateInstance(serviceProvider, typeof(TClient), invoker);

            return (TClient)client;
        }
    }
}

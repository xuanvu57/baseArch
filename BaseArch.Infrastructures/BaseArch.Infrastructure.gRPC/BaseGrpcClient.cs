using BaseArch.Infrastructure.gRPC.Interceptors;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Infrastructure.gRPC
{
    /// <summary>
    /// Base Grpc client
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="serviceProvider"></param>
    public abstract class BaseGrpcClient(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        /// <summary>
        /// Create Grpc channel with base address from configure key
        /// </summary>
        /// <param name="uriConfigKey">Key for Uri from configure file</param>
        /// <returns><see cref="GrpcChannel"/></returns>
        protected GrpcChannel CreateChannelFromConfigureKey(string uriConfigKey)
        {
            var uri = configuration[uriConfigKey] ?? "";

            return CreateChannelFromUri(uri);
        }

        /// <summary>
        /// Create Grpc channel with base address from specific Uri
        /// </summary>
        /// <param name="uri">Uri</param>
        /// <returns><see cref="GrpcChannel"/></returns>
        protected static GrpcChannel CreateChannelFromUri(string uri)
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

        /// <summary>
        /// Create Grpc client instance with default interceptors
        /// </summary>
        /// <typeparam name="TClient">Type of Grpc client</typeparam>
        /// <param name="channel"><see cref="GrpcChannel"/></param>
        /// <param name="clientInterceptors">List of <see cref="Interceptor"/></param>
        /// <returns>Grpc client</returns>
        protected TClient CreateGrpcClient<TClient>(GrpcChannel channel, params Interceptor[] clientInterceptors) where TClient : class
        {
            var grpcClientLoggingInterceptor = ActivatorUtilities.CreateInstance(serviceProvider, typeof(GrpcClientLoggingInterceptor));
            var grpcClientCorrelationIdInterceptor = ActivatorUtilities.CreateInstance(serviceProvider, typeof(GrpcClientCorrelationIdInterceptor));
            var grpcClientAuthenticationInterceptor = ActivatorUtilities.CreateInstance(serviceProvider, typeof(GrpcClientAuthenticationInterceptor));

            var invoker = channel
                .Intercept((Interceptor)grpcClientCorrelationIdInterceptor)
                .Intercept((Interceptor)grpcClientAuthenticationInterceptor)
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

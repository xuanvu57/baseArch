using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace BaseArch.Infrastructure.gRPC
{
    public abstract class BaseGrpcClient(IConfiguration configuration)
    {
        public GrpcChannel CreateChannelFromConfigureKey(string appConfigKey)
        {
            var uri = configuration.GetValue<string>(appConfigKey) ?? "";

            return CreateChannelFromUri(uri);
        }

        public GrpcChannel CreateChannelFromUri(string uri)
        {
            var uriAddress = new Uri(uri);
            var httpHandler = new HttpClientHandler();
            if (uriAddress.Scheme == "http")
            {
                httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }
            return GrpcChannel.ForAddress(uriAddress, new GrpcChannelOptions { HttpHandler = httpHandler });
        }
    }
}

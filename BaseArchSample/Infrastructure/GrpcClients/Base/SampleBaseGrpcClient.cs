using BaseArch.Infrastructure.gRPC.Clients;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.GrpcClients.Base
{
    public abstract class SampleBaseGrpcClient(IConfiguration configuration, IServiceProvider serviceProvider) : BaseGrpcClient(configuration, serviceProvider)
    {
        private const string appConfigKey = "Services:BaseArchSample:Url";

        protected GrpcChannel CreateChannelFromConfigureKey()
        {
            return CreateChannelFromConfigureKey(appConfigKey);
        }
    }
}

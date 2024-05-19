using BaseArch.Infrastructure.gRPC;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.GrpcClients
{
    public class SampleBaseGrpcClient(IConfiguration configuration) : BaseGrpcClient(configuration)
    {
        private const string appConfigKey = "Services:BaseArchSample:Url";

        protected GrpcChannel CreateChannelFromConfigureKey()
        {
            return CreateChannelFromConfigureKey(appConfigKey);
        }
    }
}

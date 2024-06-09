using Application.User.ExternalServices.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArchSample.Shares.gRPC;
using Infrastructure.GrpcClients.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.GrpcClients
{
    [DIService(DIServiceLifetime.Scoped)]
    public class GreetingClient(ILogger<GreetingClient> logger, IConfiguration configuration, IServiceProvider serviceProvider) : SampleBaseGrpcClient(configuration, serviceProvider), IGreetingClient
    {
        public async Task<string> TryToSayHello(string fullName)
        {
            using var channel = CreateChannelFromConfigureKey();

            var client = CreateGrpcClient<Greeter.GreeterClient>(channel);

            var reply = await client.SayHelloAsync(new HelloRequest { Name = fullName });

            logger.LogInformation($"[GrpcClient] Grpc server response with: {reply.Message}");
            return await Task.FromResult(reply.Message);
        }
    }
}

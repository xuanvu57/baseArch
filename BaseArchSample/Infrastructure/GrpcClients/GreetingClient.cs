using Application.User.ExternalServices.Interfaces;
using BaseArch.Domain.Attributes;
using BaseArch.Domain.Enums;
using BaseArchSample.Shares.gRPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.GrpcClients
{
    [DIService(DIServiceLifetime.Scoped)]
    public class GreetingClient(ILogger<GreetingClient> logger, IConfiguration configuration) : SampleBaseGrpcClient(configuration), IGreetingClient
    {
        public async Task<string> TryToSayHello(string fullName)
        {
            using var channel = CreateChannelFromConfigureKey();

            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(new HelloRequest { Name = fullName });

            logger.LogInformation($"[Controller] Grpc server response with: {reply.Message}");
            return await Task.FromResult(reply.Message);
        }
    }
}

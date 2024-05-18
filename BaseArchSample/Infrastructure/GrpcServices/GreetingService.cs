using Application.User.Services.Interfaces;
using BaseArch.Infrastructure.gRPC.Attributes;
using BaseArchSample.Shares.gRPC;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Infrastructure.GrpcServices
{
    [GrpcService]
    public class GreetingService(ILogger<GreetingService> logger, IGetAllUsersService getAllUsersService) : Greeter.GreeterBase
    {
        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            logger.LogInformation($"[GrpcService] Received request from client: {request.Name}");

            var users = await getAllUsersService.GetAllUsers();
            var user = users.FirstOrDefault(u => u.FullName == request.Name);

            return await Task.FromResult(new HelloReply { Message = $"{user?.FullName}" });
        }
    }
}

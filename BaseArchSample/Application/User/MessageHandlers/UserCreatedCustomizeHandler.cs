using Application.User.Dtos.Messages;
using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.User.MessageHandlers
{
    [DIService(DIServiceLifetime.Scoped)]
    public class UserCreatedCustomizeHandler(ILogger<UserCreatedCustomizeHandler> logger) : IMessageHandler<UserCreatedCustomizeMessage>
    {
        public async Task Handle(UserCreatedCustomizeMessage message)
        {
            logger.LogInformation("[MessageCustomizeHandler-Publish] recieved {@Message}", message);

            await Task.CompletedTask;
        }
    }
}

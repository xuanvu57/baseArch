using Application.User.Dtos.Messages;
using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.User.MessageHandlers
{
    [DIService(DIServiceLifetime.Scoped)]
    public class UserCreatedPublishedHandler(ILogger<UserCreatedPublishedHandler> logger) : IEventMessageHandler<UserCreatedPublishedMessage>
    {
        public async Task Handle(UserCreatedPublishedMessage message)
        {
            logger.LogInformation("[MessageHandler-Publish] recieved {@Message}", message);

            await Task.CompletedTask;
        }
    }
}

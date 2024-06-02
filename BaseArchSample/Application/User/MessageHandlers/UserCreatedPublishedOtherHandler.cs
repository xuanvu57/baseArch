using Application.User.Dtos.Messages;
using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.User.MessageHandlers
{
    [DIService(DIServiceLifetime.Scoped)]
    public class UserCreatedPublishedOtherHandler(ILogger<UserCreatedPublishedOtherHandler> logger) : IMessageHandler<UserCreatedPublishedMessage>
    {
        public async Task Handle(UserCreatedPublishedMessage message)
        {
            logger.LogInformation("[MessageOtherHandler-Publish] recieved {@Message}", message);

            await Task.CompletedTask;
        }
    }
}

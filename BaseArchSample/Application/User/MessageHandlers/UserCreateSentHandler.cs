using Application.User.Dtos.Messages;
using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.User.MessageHandlers
{
    [DIService(DIServiceLifetime.Scoped)]
    public class UserCreateSentHandler(ILogger<UserCreateSentHandler> logger) : IMessageHandler<UserCreatedSentMessage>
    {
        public async Task Handle(UserCreatedSentMessage message)
        {
            logger.LogInformation("[MessageHandler-Send] recieved {@Message}", message);

            await Task.CompletedTask;
        }
    }
}

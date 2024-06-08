using Application.User.Dtos.Messages;
using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Infrastructure.MassTransit.Implementations;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Consumers
{
    public class UserCreatedCustomizeConsumer(
        ILogger<UserCreatedCustomizeConsumer> logger,
        IEventMessageHandler<UserCreatedCustomizeMessage> messageHandler) : DefaultConsumer<Batch<UserCreatedCustomizeMessage>>(logger)
    {
        public override async Task ConsumeEventMessage(ConsumeContext<Batch<UserCreatedCustomizeMessage>> context)
        {
            for (var i = 0; i < context.Message.Length; i++)
            {
                await messageHandler.Handle(context.Message[i].Message);
            }
        }
    }
}

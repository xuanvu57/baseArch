using Application.User.Dtos.Messages;
using BaseArch.Application.MessageQueues.Interfaces;
using MassTransit;

namespace Infrastructure.Consumers
{
    public class UserCreatedCustomizeConsumer(IMessageHandler<UserCreatedCustomizeMessage> messageHandler) : IConsumer<UserCreatedCustomizeMessage>
    {
        public async Task Consume(ConsumeContext<UserCreatedCustomizeMessage> context)
        {
            await messageHandler.Handle(context.Message);
        }
    }
}

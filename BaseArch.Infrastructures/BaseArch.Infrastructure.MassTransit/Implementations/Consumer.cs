using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.Timezones.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    public class Consumer<TMessage>(
        ILogger<Consumer<TMessage>> logger,
        IDateTimeProvider dateTimeProvider,
        IEnumerable<IEventMessageHandler<TMessage>> eventMessageHandlers) : DefaultConsumer<TMessage>(logger, dateTimeProvider) where TMessage : class
    {
        public override async Task ConsumeEventMessage(ConsumeContext<TMessage> context)
        {
            var tasks = new List<Task>();
            foreach (var messageHandler in eventMessageHandlers)
            {
                tasks.Add(messageHandler.Handle(context.Message));
            }

            await Task.WhenAll(tasks);
        }
    }
}

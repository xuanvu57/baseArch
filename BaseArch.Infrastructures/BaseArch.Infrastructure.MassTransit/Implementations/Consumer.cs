using BaseArch.Application.MessageQueues.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    public class Consumer<TMessage>(
        ILogger<Consumer<TMessage>> logger,
        IEnumerable<IEventMessageHandler<TMessage>> eventMessageHandlers) : DefaultConsumer<TMessage>(logger) where TMessage : class
    {
        public override async Task ConsumeEventMessage(ConsumeContext<TMessage> context)
        {
            // TODO: need to do research to check why it create and inject duplidated handlers
            var uniqueMessageHandlers = eventMessageHandlers.GroupBy(x => x.GetType()).Select(x => x.First());

            var tasks = new List<Task>();
            foreach (var messageHandler in uniqueMessageHandlers)
            {
                tasks.Add(messageHandler.Handle(context.Message));
            }

            await Task.WhenAll(tasks);
        }
    }
}

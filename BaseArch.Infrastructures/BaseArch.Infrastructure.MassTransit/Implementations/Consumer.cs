using BaseArch.Application.MessageQueues.Interfaces;
using MassTransit;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    public class Consumer<TMessage>(IEnumerable<IMessageHandler<TMessage>> messageHandlers) : IConsumer<TMessage> where TMessage : class
    {
        public async Task Consume(ConsumeContext<TMessage> context)
        {
            var uniqueMessageHandlers = messageHandlers.GroupBy(x => x.GetType()).Select(x => x.First());

            var tasks = new List<Task>();
            foreach (var messageHandler in uniqueMessageHandlers)
            {
                tasks.Add(messageHandler.Handle(context.Message));
            }

            await Task.WhenAll(tasks);
        }
    }
}

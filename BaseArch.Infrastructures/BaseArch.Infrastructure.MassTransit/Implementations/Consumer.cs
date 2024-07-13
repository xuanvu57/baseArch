using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.Timezones.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    /// <summary>
    /// The consumer which is registered automatically
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="logger"><see cref="ILogger"/></param>
    /// <param name="dateTimeProvider"><see cref="IDateTimeProvider"/></param>
    /// <param name="eventMessageHandlers">List of <see cref="IEventMessageHandler{TMessage}"/></param>
    public class Consumer<TMessage>(
        ILogger<Consumer<TMessage>> logger,
        IDateTimeProvider dateTimeProvider,
        IEnumerable<IEventMessageHandler<TMessage>> eventMessageHandlers) : DefaultConsumer<TMessage>(logger, dateTimeProvider) where TMessage : class
    {
        /// <inheritdoc/>
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

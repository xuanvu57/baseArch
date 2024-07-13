using BaseArch.Application.Loggings;
using BaseArch.Application.Loggings.Models;
using BaseArch.Domain.Timezones.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    /// <summary>
    /// An abstract consumer to define contract for consumers
    /// </summary>
    /// <typeparam name="TMessage">Type of message</typeparam>
    /// <param name="logger"><see cref="ILogger"/></param>
    /// <param name="dateTimeProvider"><see cref="IDateTimeProvider"/></param>
    public abstract class DefaultConsumer<TMessage>(ILogger<DefaultConsumer<TMessage>> logger, IDateTimeProvider dateTimeProvider) : IConsumer<TMessage> where TMessage : class
    {
        /// <summary>
        /// Consumer and handle the event message
        /// </summary>
        /// <param name="context"><see cref="ConsumeContext{T}"/></param>
        /// <returns><see cref="Task"/></returns>
        public abstract Task ConsumeEventMessage(ConsumeContext<TMessage> context);

        /// <summary>
        /// Consume and handle the message from <see cref="IConsumer"/>
        /// </summary>
        /// <param name="context"><see cref="ConsumeContext{T}"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task Consume(ConsumeContext<TMessage> context)
        {
            var startedAtUtc = dateTimeProvider.GetUtcNow();

            try
            {
                await ConsumeEventMessage(context);
            }
            finally
            {
                WriteEventMessageLog(startedAtUtc, context.Message);
            }
        }

        /// <summary>
        /// Write log for event message which arrive to consumer
        /// </summary>
        /// <param name="startedAtUtc">Time at message arrival</param>
        /// <param name="message">Message</param>
        private void WriteEventMessageLog(DateTime startedAtUtc, TMessage message)
        {
            var eventMessageLogModel = new EventMessageLogModel<TMessage>()
            {
                Message = message,
                StartedAtUtc = startedAtUtc,
                EndedAtUtc = dateTimeProvider.GetUtcNow()
            };

            logger.LogInformation(LogMessageTemplate.QueueConsumerLogTemplate, GetType().Name, eventMessageLogModel);
        }
    }
}

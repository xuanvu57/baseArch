using BaseArch.Application.Loggings;
using BaseArch.Application.Loggings.Models;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    /// <summary>
    /// An abstract producer to defince contract for producer
    /// </summary>
    /// <param name="logger"></param>
    public abstract class DefaultProducer(ILogger<DefaultProducer> logger)
    {
        /// <summary>
        /// Write log for event message which was published or sent from producer
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="startedAtUtc">Time when message was sent</param>
        /// <param name="endedAtUtc">Time when the handling already completed</param>
        /// <param name="message">Message</param>
        protected void WriteEventMessageLog<TMessage>(DateTime startedAtUtc, DateTime endedAtUtc, TMessage message) where TMessage : class
        {
            var eventMessageLogModel = new EventMessageLogModel<TMessage>()
            {
                Message = message,
                StartedAtUtc = startedAtUtc,
                EndedAtUtc = endedAtUtc
            };

            logger.LogInformation(LogMessageTemplate.QueueProducerLogTemplate, GetType().Name, eventMessageLogModel);
        }
    }
}

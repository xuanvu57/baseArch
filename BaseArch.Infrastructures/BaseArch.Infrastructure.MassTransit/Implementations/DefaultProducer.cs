using BaseArch.Application.Loggings;
using BaseArch.Application.Loggings.Models;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    public abstract class DefaultProducer(ILogger<DefaultProducer> logger)
    {
        protected void WriteEventMessageLog<TMessage>(DateTime startedAtUtc, DateTime endedAtUtc, TMessage message) where TMessage : class
        {
            var eventMessageLogModel = new EventMessageLogModel<TMessage>()
            {
                Message = message,
                StartedAtUtc = startedAtUtc,
                EndedAtUtc = endedAtUtc
            };

            logger.LogInformation(LogMessageTemplate.QueueProducer, GetType().Name, eventMessageLogModel);
        }
    }
}

using BaseArch.Application.Loggings;
using BaseArch.Application.Loggings.Models;
using BaseArch.Domain.Timezones.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    public abstract class DefaultConsumer<TMessage>(ILogger<DefaultConsumer<TMessage>> logger, IDateTimeProvider dateTimeProvider) : IConsumer<TMessage> where TMessage : class
    {
        public abstract Task ConsumeEventMessage(ConsumeContext<TMessage> context);

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

        private void WriteEventMessageLog(DateTime startedAtUtc, TMessage message)
        {
            var eventMessageLogModel = new EventMessageLogModel<TMessage>()
            {
                Message = message,
                StartedAtUtc = startedAtUtc,
                EndedAtUtc = dateTimeProvider.GetUtcNow()
            };

            logger.LogInformation(LogMessageTemplate.QueueConsumer, GetType().Name, eventMessageLogModel);
        }
    }
}

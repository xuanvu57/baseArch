using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.Timezones.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    [DIService(DIServiceLifetime.Scoped)]
    public class Publisher(
        ILogger<Publisher> logger,
        IPublishEndpoint publishEndpoint,
        IDateTimeProvider dateTimeProvider) : DefaultProducer(logger), IPublisher
    {
        public async Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class
        {
            var startedAtUtc = dateTimeProvider.GetUtcNow();

            try
            {
                await publishEndpoint.Publish(message, cancellationToken);
            }
            finally
            {
                var endedAtUtc = dateTimeProvider.GetUtcNow();
                WriteEventMessageLog(startedAtUtc, endedAtUtc, message);
            }
        }
    }
}

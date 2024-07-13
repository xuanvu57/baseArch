using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.Timezones.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    /// <summary>
    /// The concret publisher for <see cref="IPublisher"/> to publish the message
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/></param>
    /// <param name="publishEndpoint"><see cref="IPublishEndpoint"/></param>
    /// <param name="dateTimeProvider"><see cref="IDateTimeProvider"/></param>
    [DIService(DIServiceLifetime.Scoped)]
    public class Publisher(
        ILogger<Publisher> logger,
        IPublishEndpoint publishEndpoint,
        IDateTimeProvider dateTimeProvider) : DefaultProducer(logger), IPublisher
    {
        /// <inheritdoc/>
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

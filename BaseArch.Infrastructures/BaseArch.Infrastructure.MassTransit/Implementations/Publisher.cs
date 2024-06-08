using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    [DIService(DIServiceLifetime.Scoped)]
    public class Publisher(
        ILogger<Publisher> logger,
        IPublishEndpoint publishEndpoint) : DefaultProducer(logger), IPublisher
    {
        public async Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class
        {
            var startedAtUtc = DateTime.UtcNow;

            try
            {
                await publishEndpoint.Publish(message, cancellationToken);
            }
            finally
            {
                WriteEventMessageLog(startedAtUtc, message);
            }
        }
    }
}

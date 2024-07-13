using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.Timezones.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    /// <summary>
    /// The concret sender for <see cref="IPublisher"/> to sent the message
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/></param>
    /// <param name="sendEndpointProvider"><see cref="ISendEndpointProvider"/></param>
    /// <param name="dateTimeProvider"><see cref="IDateTimeProvider"/></param>
    [DIService(DIServiceLifetime.Scoped)]
    public class Sender(
        ILogger<Sender> logger,
        ISendEndpointProvider sendEndpointProvider,
        IDateTimeProvider dateTimeProvider) : DefaultProducer(logger), ISender
    {
        /// <inheritdoc/>
        public async Task Send<TMessage>(TMessage message, string uri = "", CancellationToken cancellationToken = default) where TMessage : class
        {
            var startedAtUtc = dateTimeProvider.GetUtcNow();

            try
            {
                var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri(uri));

                await sendEndpoint.Send(message, cancellationToken);
            }
            finally
            {
                var endedAtUtc = dateTimeProvider.GetUtcNow();
                WriteEventMessageLog(startedAtUtc, endedAtUtc, message);
            }
        }
    }
}

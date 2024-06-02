using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.DependencyInjection;
using MassTransit;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    [DIService(DIServiceLifetime.Scoped)]
    public class Publisher(IPublishEndpoint publishEndpoint) : IPublisher
    {
        public async Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class
        {
            await publishEndpoint.Publish(message, cancellationToken);
        }
    }
}

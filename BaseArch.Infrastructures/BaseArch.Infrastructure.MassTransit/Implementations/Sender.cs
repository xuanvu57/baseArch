using BaseArch.Application.MessageQueues.Interfaces;
using BaseArch.Domain.DependencyInjection;
using MassTransit;

namespace BaseArch.Infrastructure.MassTransit.Implementations
{
    [DIService(DIServiceLifetime.Scoped)]
    public class Sender(ISendEndpointProvider sendEndpointProvider) : ISender
    {
        public async Task Send<TMessage>(TMessage message, string uri, CancellationToken cancellationToken = default) where TMessage : class
        {
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri(uri));

            await sendEndpoint.Send(message, cancellationToken);
        }
    }
}

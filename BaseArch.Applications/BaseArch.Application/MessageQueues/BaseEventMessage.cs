using BaseArch.Application.MessageQueues.Interfaces;

namespace BaseArch.Application.MessageQueues
{
    /// <summary>
    /// Base event message for auto registration of consumers
    /// </summary>
    /// <typeparam name="TMessageId">Type of message id</typeparam>
    /// <param name="MessageId"><see cref="TMessageId"/></param>
    public record BaseEventMessage<TMessageId>(TMessageId MessageId) : IEventMessage<TMessageId>;
}

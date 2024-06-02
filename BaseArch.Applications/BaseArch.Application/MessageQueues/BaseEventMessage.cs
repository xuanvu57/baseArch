using BaseArch.Application.MessageQueues.Interfaces;

namespace BaseArch.Application.MessageQueues
{
    public record BaseEventMessage<TMessageId>(TMessageId MessageId) : IEventMessage<TMessageId>;
}

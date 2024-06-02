namespace BaseArch.Application.MessageQueues.Interfaces
{
    public interface IEventMessage<TMessageId>
    {
        public TMessageId MessageId { get; init; }
    }
}

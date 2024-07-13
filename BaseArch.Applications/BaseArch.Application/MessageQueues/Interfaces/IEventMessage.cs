namespace BaseArch.Application.MessageQueues.Interfaces
{
    /// <summary>
    /// Event message
    /// </summary>
    /// <typeparam name="TMessageId"><see cref="TMessageId"/></typeparam>
    public interface IEventMessage<TMessageId>
    {
        /// <summary>
        /// Message id
        /// </summary>
        public TMessageId MessageId { get; init; }
    }
}

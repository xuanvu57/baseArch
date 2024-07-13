namespace BaseArch.Application.MessageQueues.Interfaces
{
    /// <summary>
    /// Event message handler interface
    /// </summary>
    /// <typeparam name="TMessage"><see cref="TMessage"/></typeparam>
    public interface IEventMessageHandler<in TMessage> where TMessage : class
    {
        /// <summary>
        /// Handle the message
        /// </summary>
        /// <param name="message"><see cref="TMessage"/></param>
        /// <returns><see cref="Task"/></returns>
        Task Handle(TMessage message);
    }
}

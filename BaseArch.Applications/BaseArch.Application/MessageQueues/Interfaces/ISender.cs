namespace BaseArch.Application.MessageQueues.Interfaces
{
    /// <summary>
    /// Sender interface
    /// </summary>
    public interface ISender
    {
        /// <summary>
        /// Send message to queue
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="message">The message will be sent</param>
        /// <param name="uri">Endpoint which message will be sent to</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task Send<TMessage>(TMessage message, string uri = "", CancellationToken cancellationToken = default) where TMessage : class;
    }
}

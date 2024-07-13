namespace BaseArch.Application.MessageQueues.Interfaces{
    /// <summary>
    /// Publisher interface
    /// </summary>                                          public interface IPublisher    {
        /// <summary>
        /// Publish the message to queue
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="message">The message will be published</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>                                                          Task Publish<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class;
    }}
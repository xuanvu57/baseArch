namespace BaseArch.Application.MessageQueues.Interfaces
{
    public interface ISender
    {
        Task Send<TMessage>(TMessage message, string uri, CancellationToken cancellationToken = default) where TMessage : class;
    }
}

namespace BaseArch.Application.MessageQueues.Interfaces
{
    public interface IMessageHandler<in TMessage> where TMessage : class
    {
        Task Handle(TMessage message);
    }
}

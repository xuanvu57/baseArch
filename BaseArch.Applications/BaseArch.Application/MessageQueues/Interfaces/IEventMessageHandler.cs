namespace BaseArch.Application.MessageQueues.Interfaces
{
    public interface IEventMessageHandler<in TMessage> where TMessage : class
    {
        Task Handle(TMessage message);
    }
}

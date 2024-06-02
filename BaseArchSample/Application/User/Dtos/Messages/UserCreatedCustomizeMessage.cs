using BaseArch.Application.MessageQueues.Interfaces;

namespace Application.User.Dtos.Messages
{
    public record UserCreatedCustomizeMessage(Guid MessageId, Guid UserId) : IEventMessage<Guid>;
}

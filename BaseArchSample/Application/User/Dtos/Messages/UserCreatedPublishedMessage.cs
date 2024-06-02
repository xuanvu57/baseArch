using BaseArch.Application.MessageQueues;

namespace Application.User.Dtos.Messages
{
    public record UserCreatedPublishedMessage(Guid UserId) : BaseEventMessage<Guid>(Guid.NewGuid());
}

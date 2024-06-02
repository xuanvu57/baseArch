using BaseArch.Application.MessageQueues;

namespace Application.User.Dtos.Messages
{
    public record UserCreatedSentMessage(Guid UserId, string FullName) : BaseEventMessage<Guid>(Guid.NewGuid());
}

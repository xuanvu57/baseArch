namespace BaseArch.Application.Loggings.Models
{
    public record EventMessageLogModel<TMessage>
    {
        public required TMessage Message { get; init; }

        public required DateTime StartedAtUtc { get; init; }

        public required DateTime EndedAtUtc { get; init; }
    }
}

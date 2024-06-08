namespace BaseArch.Infrastructure.MassTransit.Options
{
    public record InMemoryQueueOptions
    {
        public required bool Enable { get; init; }
    }
}

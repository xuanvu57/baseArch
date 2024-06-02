namespace BaseArch.Infrastructure.MassTransit.Options
{
    public record MessageQueuesOptions
    {
        public bool InMemoryQueue { get; init; }
        public RabbitMqOptions? RabbitMq { get; init; }
        public RetryOptions? Retry { get; init; }
    }
}

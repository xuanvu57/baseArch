namespace BaseArch.Infrastructure.MassTransit.Options
{
    public record MassTransitOptions
    {
        public InMemoryQueueOptions? InMemoryQueue { get; init; }
        public RabbitMqOptions? RabbitMq { get; init; }
        public EndPointFormatterOptions? EndPointFormatter { get; init; }
        public RetryOptions? Retry { get; init; }
    }
}

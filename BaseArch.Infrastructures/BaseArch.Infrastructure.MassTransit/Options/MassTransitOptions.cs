namespace BaseArch.Infrastructure.MassTransit.Options
{
    /// <summary>
    /// MassTransit options
    /// </summary>
    public sealed record MassTransitOptions
    {
        /// <see cref="InMemoryQueueOptions"/>
        public InMemoryQueueOptions? InMemoryQueue { get; init; }

        /// <see cref="RabbitMqOptions"/>
        public RabbitMqOptions? RabbitMq { get; init; }

        /// <see cref="EndPointFormatterOptions"/>
        public EndPointFormatterOptions? EndPointFormatter { get; init; }

        /// <see cref="RetryOptions"/>
        public RetryOptions? Retry { get; init; }
    }
}

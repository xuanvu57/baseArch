namespace BaseArch.Infrastructure.MassTransit.Options
{
    public record RabbitMqOptions
    {
        public required bool Enable { get; init; }
        public required string Server { get; init; }
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}

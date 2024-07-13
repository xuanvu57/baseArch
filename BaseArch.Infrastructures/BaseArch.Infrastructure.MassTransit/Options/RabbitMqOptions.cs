namespace BaseArch.Infrastructure.MassTransit.Options
{
    /// <summary>
    /// RabbitMq options
    /// </summary>
    public sealed record RabbitMqOptions
    {
        /// <summary>
        /// Enable
        /// </summary>
        public bool Enable { get; init; }

        /// <summary>
        /// RabbitMq server host
        /// </summary>
        public required string Server { get; init; }

        /// <summary>
        /// Username for credential
        /// </summary>
        public required string Username { get; init; }

        /// <summary>
        /// Password for credential
        /// </summary>
        public required string Password { get; init; }
    }
}

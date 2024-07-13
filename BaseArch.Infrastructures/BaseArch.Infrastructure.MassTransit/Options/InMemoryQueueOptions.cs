namespace BaseArch.Infrastructure.MassTransit.Options
{
    /// <summary>
    /// In-memory queue options
    /// </summary>
    public sealed record InMemoryQueueOptions
    {
        /// <summary>
        /// Enable
        /// </summary>
        public bool Enable { get; init; }
    }
}

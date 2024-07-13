namespace BaseArch.Infrastructure.MassTransit.Options
{
    /// <summary>
    /// Retry options
    /// </summary>
    public sealed record RetryOptions
    {
        /// <summary>
        /// Duration between retry times (in second)
        /// </summary>
        public int IntevalInSecond { get; init; }

        /// <summary>
        /// Maximum times to retry
        /// </summary>
        public int MaxTimes { get; init; }
    }
}

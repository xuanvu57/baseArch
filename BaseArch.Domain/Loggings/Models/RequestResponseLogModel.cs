namespace BaseArch.Domain.Loggings.Models
{
    /// <summary>
    /// Basic model for request logging
    /// </summary>
    public record RequestResponseLogModel
    {
        /// <summary>
        /// <see cref="RequestLogModel"/>
        /// </summary>
        public required RequestLogModel RequestLogModel { get; init; }

        /// <summary>
        /// <see cref="ResponseLogModel"/>
        /// </summary>
        public required ResponseLogModel ResponseLogModel { get; init; }
    }
}

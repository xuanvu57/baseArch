namespace BaseArch.Domain.Loggings.Models
{
    /// <summary>
    /// Response model for logging
    /// </summary>
    public record ResponseLogModel
    {
        /// <summary>
        /// Time after process the request
        /// </summary>
        public DateTime TimeUtc { get; init; }

        /// <summary>
        /// Http code status
        /// </summary>
        public required string Status { get; init; }

        /// <summary>
        /// Response content type
        /// </summary>
        public required string ContentType { get; init; }

        /// <summary>
        /// List of response's headers
        /// </summary>
        public required Dictionary<string, string> Header { get; init; }

        /// <summary>
        /// Response body in string
        /// </summary>
        public required string Body { get; init; }
    }
}

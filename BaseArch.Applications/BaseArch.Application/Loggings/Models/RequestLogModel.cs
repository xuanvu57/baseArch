namespace BaseArch.Application.Loggings.Models
{
    /// <summary>
    /// Request model for logging
    /// </summary>
    public record RequestLogModel
    {
        /// <summary>
        /// Time before process the request
        /// </summary>
        public DateTime TimeUtc { get; init; }

        /// <summary>
        /// Request scheme (usually http or https)
        /// </summary>
        public required string Scheme { get; init; }

        /// <summary>
        /// Http method
        /// </summary>
        public required string Method { get; init; }

        /// <summary>
        /// Uri path
        /// </summary>
        public required string Path { get; init; }

        /// <summary>
        /// Request content type
        /// </summary>
        public required string ContentType { get; init; }

        /// <summary>
        /// List of request's headers
        /// </summary>
        public required Dictionary<string, string> Headers { get; init; }

        /// <summary>
        /// Query string
        /// </summary>
        public required string QueryString { get; init; }

        /// <summary>
        /// Request body as string
        /// </summary>
        public required string Body { get; init; }
    }
}

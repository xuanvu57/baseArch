namespace BaseArch.Application.CorrelationId
{
    /// <summary>
    /// Options for using correlation id
    /// </summary>
    public sealed class CorrelationIdOptions
    {
        /// <summary>
        /// The default header used for correlation id
        /// </summary>
        private const string DefaultHeader = "X-Correlation-Id";

        /// <summary>
        /// The name of the header from which the Correlation id is read from the request
        /// </summary>
        public string RequestHeader { get; set; } = DefaultHeader;

        /// <summary>
        /// The name of the header from which the Correlation id is written for the response
        /// </summary>
        public string ResponseHeader { get; set; } = DefaultHeader;
    }
}

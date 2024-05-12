using BaseArch.Application.CorrelationId.Interfaces;

namespace BaseArch.Application.CorrelationId
{
    /// <summary>
    /// Correlation id provider as GUID
    /// </summary>
    public class GuidCorrelationIdProvider : ICorrelationIdProvider
    {
        /// <summary>
        /// Current correlation id
        /// </summary>
        private Guid CorrelationId { get; set; } = Guid.Empty;

        /// <inheritdoc/>
        public string Generate()
        {
            return Guid.NewGuid().ToString();
        }

        /// <inheritdoc/>
        public string Get()
        {
            return CorrelationId.ToString();
        }

        /// <inheritdoc/>
        public void Set(string correlationId)
        {
            CorrelationId = Guid.Parse(correlationId);
        }
    }
}

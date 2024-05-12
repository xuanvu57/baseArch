namespace BaseArch.Application.CorrelationId.Interfaces
{
    /// <summary>
    /// Correlation id provider interface
    /// </summary>
    public interface ICorrelationIdProvider
    {
        /// <summary>
        /// Get the current correlation id
        /// </summary>
        /// <returns>Correction id</returns>
        string Get();

        /// <summary>
        /// Generate a new correlation id as a string
        /// </summary>
        /// <returns>New correlation id</returns>
        string Generate();

        /// <summary>
        /// Set correlation id to current
        /// </summary>
        /// <param name="correlationId">Correlation id to set</param>
        void Set(string correlationId);
    }
}

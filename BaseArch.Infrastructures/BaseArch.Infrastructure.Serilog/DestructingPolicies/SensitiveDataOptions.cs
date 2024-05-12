namespace BaseArch.Infrastructure.Serilog.DestructingPolicies
{
    /// <summary>
    /// Sensitive data option
    /// </summary>
    public sealed class SensitiveDataOptions
    {
        /// <summary>
        /// Default mask value
        /// </summary>
        private const string DefaultMaskValue = "**MASKED**";

        /// <summary>
        /// Mask value
        /// </summary>
        public string MaskValue { get; set; } = DefaultMaskValue;

        /// <summary>
        /// List of keywords for sensitive data
        /// </summary>
        public string[] Keywords { get; set; } = [];
    }
}

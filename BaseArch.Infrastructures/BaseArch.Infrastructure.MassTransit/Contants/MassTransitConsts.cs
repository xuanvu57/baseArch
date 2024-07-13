namespace BaseArch.Infrastructure.MassTransit.Contants
{
    /// <summary>
    /// MassTransit constants
    /// </summary>
    public static class MassTransitConsts
    {
        /// <summary>
        /// Configuration section for MassTransit
        /// </summary>
        public const string MassTransitSection = "MassTransit";

        /// <summary>
        /// Enum for Endpoint formatter
        /// </summary>
        public enum EndPointFormatterCaseEnums
        {
            KebabCase,
            SnakeCase
        }
    }
}

using static BaseArch.Infrastructure.MassTransit.Contants.MassTransitConsts;

namespace BaseArch.Infrastructure.MassTransit.Options
{
    /// <summary>
    /// Endpoint formatter options
    /// </summary>
    public sealed record EndPointFormatterOptions
    {
        /// <summary>
        /// Endpoint formatter case
        /// </summary>
        public EndPointFormatterCaseEnums EndPointFormatterCase { get; init; }

        /// <summary>
        /// Endpoint prefix
        /// </summary>
        public string Prefix { get; init; } = string.Empty;
    }
}

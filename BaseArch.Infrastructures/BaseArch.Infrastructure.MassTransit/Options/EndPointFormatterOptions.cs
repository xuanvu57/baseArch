using static BaseArch.Infrastructure.MassTransit.Options.MassTransitConstants;

namespace BaseArch.Infrastructure.MassTransit.Options
{
    public record EndPointFormatterOptions
    {
        public EndPointFormatterCaseEnums EndPointFormatterCase { get; init; }
        public string Prefix { get; init; } = string.Empty;
    }
}

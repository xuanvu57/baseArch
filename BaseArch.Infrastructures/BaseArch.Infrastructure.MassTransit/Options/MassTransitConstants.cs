namespace BaseArch.Infrastructure.MassTransit.Options
{
    public static class MassTransitConstants
    {
        public const string MassTransitSection = "MassTransit";

        public enum EndPointFormatterCaseEnums
        {
            KebabCase,
            SnakeCase
        }
    }
}

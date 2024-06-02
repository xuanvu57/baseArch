namespace BaseArch.Infrastructure.MassTransit.Options
{
    public record RetryOptions
    {
        public int IntevalInSecond { get; init; }
        public int MaxTimes { get; init; }
    }
}

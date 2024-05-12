namespace BaseArch.Presentation.RestApi.Models
{
    internal record RequestLogModel
    {
        public DateTime TimeUtc { get; init; }
        public required string Method { get; init; }
        public required string Path { get; init; }
        public required string ContentType { get; init; }
        public required Dictionary<string, string> Headers { get; init; }
        public required string QueryString { get; init; }
        public required string Body { get; init; }
    }
}

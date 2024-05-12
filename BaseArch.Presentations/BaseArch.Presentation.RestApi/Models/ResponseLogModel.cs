namespace BaseArch.Presentation.RestApi.Models
{
    internal record ResponseLogModel
    {
        public DateTime TimeUtc { get; init; }
        public required string Status { get; init; }
        public required string ContentType { get; init; }
        public required Dictionary<string, string> Header { get; init; }
        public required string Body { get; init; }
    }
}

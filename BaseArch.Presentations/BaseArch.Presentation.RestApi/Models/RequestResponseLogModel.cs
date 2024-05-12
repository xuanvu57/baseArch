namespace BaseArch.Presentation.RestApi.Models
{
    internal record RequestResponseLogModel
    {
        public required RequestLogModel RequestLogModel { get; init; }
        public required ResponseLogModel ResponseLogModel { get; init; }
    }
}

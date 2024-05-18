namespace BaseArch.Domain.Models
{
    public record RequestResponseLogModel
    {
        public required RequestLogModel RequestLogModel { get; init; }
        public required ResponseLogModel ResponseLogModel { get; init; }
    }
}

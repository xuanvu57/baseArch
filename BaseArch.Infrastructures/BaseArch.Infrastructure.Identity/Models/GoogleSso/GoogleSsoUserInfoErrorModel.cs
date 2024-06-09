namespace BaseArch.Infrastructure.Identity.Models.GoogleSso
{
    public record GoogleSsoUserInfoErrorModel
    {
        public int Code { get; set; }
        public required string Message { get; set; }
        public required string Status { get; set; }
    }
}

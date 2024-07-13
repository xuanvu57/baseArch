namespace BaseArch.Infrastructure.Identity.Sso.Google.Models
{
    /// <summary>
    /// Error model when get user information
    /// </summary>
    public sealed record GoogleSsoUserInfoErrorModel
    {
        /// <summary>
        /// Error code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public required string Message { get; set; }

        /// <summary>
        /// Error status
        /// </summary>
        public required string Status { get; set; }
    }
}

namespace BaseArch.Infrastructure.Identity.Constants
{
    /// <summary>
    /// Contants for identity settings
    /// </summary>
    internal static class IdentityConst
    {
        /// <summary>
        /// Configuration section for Jwt
        /// </summary>
        public const string JwtSection = "Identity:Jwt";

        /// <summary>
        /// Configuration section for Google Single sign-on
        /// </summary>
        public const string GoogleSsoSection = "Identity:GoogleSso";

        /// <summary>
        /// Configuration section for Facebook Single sign-on
        /// </summary>
        public const string FacebookSsoSection = "Identity:FacebookSso";

        /// <summary>
        /// Google SSO provider name
        /// </summary>
        public const string GoogleSsoProviderName = "Google";

        /// <summary>
        /// Facebook SSO provider name
        /// </summary>
        public const string FacebookSsoProviderName = "Facebook";
    }
}

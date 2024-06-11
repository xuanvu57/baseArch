namespace BaseArch.Infrastructure.Identity.Options
{
    /// <summary>
    /// Google single sign-on option
    /// </summary>
    public record GoogleSsoOptions
    {
        /// <summary>
        /// Enable to use Google single sign-on
        /// </summary>
        public bool Enable { get; init; }

        /// <summary>
        /// Redirect url to attach the authentication code after Google login
        /// </summary>
        public string RedirectUrl { get; init; } = "";

        /// <summary>
        /// Callback url to redirect after the single sign-on completed
        /// </summary>
        public string CallbackUrl { get; init; } = "";

        /// <summary>
        /// Client id from Google authentication configuration
        /// </summary>
        public string ClientId { get; init; } = "";

        /// <summary>
        /// Client secrect from Google authentication configuation
        /// </summary>
        public string ClientSecret { get; init; } = "";

        /// <summary>
        /// Google login url
        /// </summary>
        public string GoogleLoginUrl { get; init; } = "";

        /// <summary>
        /// OAuth Google api url
        /// </summary>
        public string OAuthGoogleApiUrl { get; init; } = "";

        /// <summary>
        /// Google api url
        /// </summary>
        public string GoogleApiUrl { get; init; } = "";
    }
}

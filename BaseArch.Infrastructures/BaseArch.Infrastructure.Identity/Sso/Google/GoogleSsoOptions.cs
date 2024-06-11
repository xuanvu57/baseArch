namespace BaseArch.Infrastructure.Identity.Sso.Google
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
        /// Client id from Google authentication configuration
        /// </summary>
        public string ClientId { get; init; } = string.Empty;

        /// <summary>
        /// Client secrect from Google authentication configuation
        /// </summary>
        public string ClientSecret { get; init; } = string.Empty;

        /// <summary>
        /// Redirect url to attach the authentication code after user loged in Google
        /// </summary>
        public string RedirectUrl { get; init; } = string.Empty;

        /// <summary>
        /// Callback url to redirect after the single sign-on completed
        /// </summary>
        public string CallbackUrl { get; init; } = string.Empty;

        /// <summary>
        /// Google login url
        /// </summary>
        public string GoogleLoginUrl { get; init; } = string.Empty;

        /// <summary>
        /// OAuth Google api url
        /// </summary>
        public string OAuthGoogleApiUrl { get; init; } = string.Empty;

        /// <summary>
        /// Google api url to get user information
        /// </summary>
        public string GoogleApiUrl { get; init; } = string.Empty;

        /// <summary>
        /// Define the fileds to get information
        /// </summary>
        public string Scope { get; init; } = "email profile";
    }
}

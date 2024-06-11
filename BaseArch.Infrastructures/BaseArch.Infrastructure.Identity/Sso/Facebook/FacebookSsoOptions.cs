namespace BaseArch.Infrastructure.Identity.Sso.Facebook
{
    public record FacebookSsoOptions
    {
        /// <summary>
        /// Enable to use Facebook single sign-on
        /// </summary>
        public bool Enable { get; init; }

        /// <summary>
        /// App id from Facebook authentication configuration
        /// </summary>
        public string AppId { get; init; } = string.Empty;

        /// <summary>
        /// App secrect from Facebook authentication configuation
        /// </summary>
        public string AppSecret { get; init; } = string.Empty;

        /// <summary>
        /// The key to maintain state between request and callback
        /// </summary>
        public string State { get; init; } = string.Empty;

        /// <summary>
        /// Redirect url to attach the authentication code after user logged in Facebook
        /// </summary>
        public string RedirectUrl { get; init; } = string.Empty;

        /// <summary>
        /// Callback url to redirect after the single sign-on completed
        /// </summary>
        public string CallbackUrl { get; init; } = string.Empty;

        /// <summary>
        /// Facebook login url
        /// </summary>
        public string FacebookLoginUrl { get; init; } = string.Empty;

        /// <summary>
        /// OAuth Facebook api url
        /// </summary>
        public string OAuthFacebookApiUrl { get; init; } = string.Empty;

        /// <summary>
        /// Facebook api url to get user information
        /// </summary>
        public string FacebookApiUrl { get; init; } = string.Empty;

        /// <summary>
        /// Define the fileds to get information
        /// </summary>
        public string Fields { get; init; } = "id,name,first_name,last_name,email,picture";
    }
}

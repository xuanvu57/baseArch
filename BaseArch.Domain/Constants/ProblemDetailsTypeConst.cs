namespace BaseArch.Domain.Constants
{
    /// <summary>
    /// Constants for problem details
    /// </summary>
    public static class ProblemDetailsTypeConst
    {
        /// <summary>
        /// 404 Bad Request
        /// </summary>
        public const string Type400BadRequest = "https://tools.ietf.org/html/rfc9110#section-15.5.1";

        /// <summary>
        /// 401 Unauthorized
        /// </summary>
        public const string Type401Unauthorized = "https://tools.ietf.org/html/rfc9110#section-15.5.2";

        /// <summary>
        /// 403 Forbidden
        /// </summary>
        public const string Type403Forbidden = "https://tools.ietf.org/html/rfc9110#section-15.5.4";

        /// <summary>
        /// 408 Request timeout
        /// </summary>
        public const string Type408RequestTimeout = "https://tools.ietf.org/html/rfc9110#section-15.5.9";

        /// <summary>
        /// 500 Internal Server Error
        /// </summary>
        public const string Type500InternalServerError = "https://tools.ietf.org/html/rfc9110#section-15.6.`";
    }
}

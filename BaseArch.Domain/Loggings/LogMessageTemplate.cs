namespace BaseArch.Domain.Loggings
{
    /// <summary>
    /// Standard message template for logging
    /// </summary>
    public static class LogMessageTemplate
    {
        /// <summary>
        /// Http request and response logging middleware
        /// </summary>
        public const string HttpRequestResponseLoggingMiddleware = "ApiService {HttpMethod} {RequestPath} responded {StatusCode} with {@RequestLogModel} {@ResponseLogModel}";

        /// <summary>
        /// Http client logging delegating handler
        /// </summary>
        public const string HttpClientLoggingDelegatingHandler = "HttpClient {HttpMethod} {RequestPath} responded {StatusCode} with {@RequestLogModel} {@ResponseLogModel}";

        /// <summary>
        /// Grpc service logging interceptor
        /// </summary>
        public const string GrpcServiceLoggingInterceptor = "GrpcService {GrpcTyp} {GrpcServiceMethod} responded {StatusCode} with {@RequestLogModel} {@ResponseLogModel}";

        /// <summary>
        /// Grpc client logging interceptor
        /// </summary>
        public const string GrpcClientLoggingInterceptor = "GrpcClient {GrpcType} {GrpcServiceMethod} responded {StatusCode} with {@RequestLogModel} {@ResponseLogModel}";
    }
}

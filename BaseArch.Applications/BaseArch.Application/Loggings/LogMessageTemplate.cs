namespace BaseArch.Application.Loggings
{
    /// <summary>
    /// Standard message template for logging
    /// </summary>
    public static class LogMessageTemplate
    {
        /// <summary>
        /// Log template for HttpRequest and HttpResponse at server
        /// </summary>
        public const string HttpRequestResponseLogTemplate = "ApiService {HttpMethod} {RequestPath} responded {StatusCode} with {@RequestLogModel} {@ResponseLogModel}";

        /// <summary>
        /// Log template for HttpRequest and HttpResponse at client
        /// </summary>
        public const string HttpClientLogTemplate = "HttpClient {HttpMethod} {RequestPath} responded {StatusCode} with {@RequestLogModel} {@ResponseLogModel}";

        /// <summary>
        /// Log template for GRPC service
        /// </summary>
        public const string GrpcServiceLogTemplate = "GrpcService {GrpcType} {GrpcServiceMethod} responded {StatusCode} with {@RequestLogModel} {@ResponseLogModel}";

        /// <summary>
        /// Log template for GRPC client
        /// </summary>
        public const string GrpcClientLogTemplate = "GrpcClient {GrpcType} {GrpcServiceMethod} responded {StatusCode} with {@RequestLogModel} {@ResponseLogModel}";

        /// <summary>
        /// Log template for consumer
        /// </summary>
        public const string QueueConsumerLogTemplate = "QueueConsumer {Consumer} completed with {@EventMessageLogModel}";

        /// <summary>
        /// Log template for producer
        /// </summary>
        public const string QueueProducerLogTemplate = "QueueProducer {Producer} completed with {@EventMessageLogModel}";
    }
}

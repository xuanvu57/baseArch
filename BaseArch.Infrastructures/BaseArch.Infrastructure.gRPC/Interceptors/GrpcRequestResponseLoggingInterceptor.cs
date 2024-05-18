using BaseArch.Domain.Models;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace BaseArch.Infrastructure.gRPC.Interceptors
{
    public class GrpcRequestResponseLoggingInterceptor(ILogger<GrpcRequestResponseLoggingInterceptor> logger) : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var responseBodyText = request.ToString() ?? "";
            try
            {
                var response = await continuation(request, context);
                responseBodyText = response.ToString() ?? "";
                return response;
            }
            finally
            {
                var httpContext = context.GetHttpContext();
                var requestLog = ExtractFromRequest(httpContext, request);
                var responseLog = ExtractFromResponse(httpContext, responseBodyText);

                var requestResponseLogModel = new RequestResponseLogModel()
                {
                    RequestLogModel = requestLog,
                    ResponseLogModel = responseLog
                };

                WriteRequestResponseLog(logger, requestResponseLogModel, context);
            }
        }

        /// <summary>
        /// Write log
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="requestResponseLogModel"><see cref="RequestResponseLogModel"/></param>
        private static void WriteRequestResponseLog(ILogger<GrpcRequestResponseLoggingInterceptor> logger, RequestResponseLogModel requestResponseLogModel, ServerCallContext context)
        {
            logger.LogInformation("Grpc {GrpcMethod} {GrpcServiceMethod} responded {StatusCode} with {@RequestLogModel} {@ResponseLogModel}",
                MethodType.Unary,
                context.Method,
                requestResponseLogModel.ResponseLogModel.Status,
                requestResponseLogModel.RequestLogModel,
                requestResponseLogModel.ResponseLogModel);
        }

        /// <summary>
        /// Extract data from <see cref="HttpResponse"/>
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <param name="responseBodyText">Response body in string</param>
        /// <returns><see cref="ResponseLogModel"/></returns>
        private static ResponseLogModel ExtractFromResponse(HttpContext httpContext, string responseBodyText)
        {
            return new ResponseLogModel()
            {
                ContentType = httpContext.Response.ContentType ?? "",
                Header = FormatHeaders(httpContext.Response.Headers),
                Body = responseBodyText,
                Status = httpContext.Response.StatusCode.ToString(),
                TimeUtc = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Extract data from <see cref="HttpRequest"/>
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <param name="request">grpc request</param>
        /// <returns><see cref="RequestLogModel"/></returns>
        private static RequestLogModel ExtractFromRequest<TRequest>(HttpContext httpContext, TRequest request)
        {
            return new RequestLogModel()
            {
                TimeUtc = DateTime.UtcNow,
                ContentType = httpContext.Request.ContentType ?? "",
                Scheme = httpContext.Request.Scheme,
                Method = httpContext.Request.Method,
                Path = httpContext.Request.Path,
                Headers = FormatHeaders(httpContext.Request.Headers),
                QueryString = httpContext.Request.QueryString.ToString(),
                Body = request?.ToString() ?? string.Empty
            };
        }

        /// <summary>
        /// Convert request/response headers to Dictionary
        /// </summary>
        /// <param name="headers"><see cref="IHeaderDictionary"/></param>
        /// <returns>Header in dictionary</returns>
        private static Dictionary<string, string> FormatHeaders(IHeaderDictionary headers)
        {
            var pairs = new Dictionary<string, string>();
            foreach (var header in headers)
            {
                pairs.Add(header.Key, StringValues.IsNullOrEmpty(header.Value) ? "" : header.Value.ToString());
            }
            return pairs;
        }
    }
}

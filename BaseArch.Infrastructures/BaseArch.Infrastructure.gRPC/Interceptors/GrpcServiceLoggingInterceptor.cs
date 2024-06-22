using BaseArch.Application.Loggings;
using BaseArch.Application.Loggings.Models;
using BaseArch.Domain.Timezones.Interfaces;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace BaseArch.Infrastructure.gRPC.Interceptors
{
    /// <summary>
    /// Interceptor to log the request and response for gRPC request
    /// </summary>
    /// <param name="logger"></param>
    public class GrpcServiceLoggingInterceptor(ILogger<GrpcServiceLoggingInterceptor> logger, IDateTimeProvider dateTimeProvider) : Interceptor
    {
        /// <inheritdoc />
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var responseBodyText = "";
            var httpContext = context.GetHttpContext();
            var requestLog = ExtractFromRequest(httpContext, request);

            try
            {
                var response = await continuation(request, context);
                responseBodyText = response.ToString() ?? "";
                return response;
            }
            finally
            {
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
        /// <param name="context"><see cref="ServerCallContext"/></param>
        private static void WriteRequestResponseLog(ILogger<GrpcServiceLoggingInterceptor> logger, RequestResponseLogModel requestResponseLogModel, ServerCallContext context)
        {
            logger.LogInformation(LogMessageTemplate.GrpcServiceLoggingInterceptor,
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
        private ResponseLogModel ExtractFromResponse(HttpContext httpContext, string responseBodyText)
        {
            return new ResponseLogModel()
            {
                ContentType = httpContext.Response.ContentType ?? "",
                Header = FormatHeaders(httpContext.Response.Headers),
                Body = responseBodyText,
                Status = httpContext.Response.StatusCode.ToString(),
                TimeUtc = dateTimeProvider.GetUtcNow()
            };
        }

        /// <summary>
        /// Extract data from <see cref="HttpRequest"/>
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <param name="request">grpc request</param>
        /// <returns><see cref="RequestLogModel"/></returns>
        private RequestLogModel ExtractFromRequest<TRequest>(HttpContext httpContext, TRequest request)
        {
            return new RequestLogModel()
            {
                TimeUtc = dateTimeProvider.GetUtcNow(),
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

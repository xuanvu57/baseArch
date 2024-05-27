using BaseArch.Domain.Loggings;
using BaseArch.Domain.Loggings.Models;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Net;

namespace BaseArch.Infrastructure.gRPC.Interceptors
{
    public class GrpcClientLoggingInterceptor(ILogger<GrpcClientLoggingInterceptor> logger) : Interceptor
    {
        /// <inheritdoc/>
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var call = continuation(request, context);

            return new AsyncUnaryCall<TResponse>(
                HandleResponse(request, call.ResponseAsync, context),
                call.ResponseHeadersAsync,
                call.GetStatus,
                call.GetTrailers,
                call.Dispose);
        }

        private async Task<TResponse> HandleResponse<TRequest, TResponse>(TRequest request, Task<TResponse> responseAsync, ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class
        {
            var responseBodyText = "";
            var requestLog = ExtractFromRequest(context, request);

            try
            {
                var response = await responseAsync;
                responseBodyText = response?.ToString() ?? "";

                return response;
            }
            finally
            {
                var responseLog = ExtractFromResponse(context, responseBodyText);

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
        private static void WriteRequestResponseLog<TRequest, TResponse>(ILogger<GrpcClientLoggingInterceptor> logger, RequestResponseLogModel requestResponseLogModel, ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class
        {
            logger.LogInformation(LogMessageTemplate.GrpcClientLoggingInterceptor,
                MethodType.Unary,
                context.Method.Name,
                requestResponseLogModel.ResponseLogModel.Status,
                requestResponseLogModel.RequestLogModel,
                requestResponseLogModel.ResponseLogModel);
        }

        /// <summary>
        /// Extract data from <see cref="HttpResponse"/>
        /// </summary>
        /// <param name="context"><see cref="ClientInterceptorContext"/></param>
        /// <param name="responseBodyText">Response body in string</param>
        /// <returns><see cref="ResponseLogModel"/></returns>
        private static ResponseLogModel ExtractFromResponse<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, string responseBodyText)
            where TRequest : class
            where TResponse : class
        {
            return new ResponseLogModel()
            {
                ContentType = "",
                Header = FormatHeaders(context.Options.Headers),
                Body = responseBodyText,
                Status = HttpStatusCode.OK.ToString(),
                TimeUtc = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Extract data from <see cref="HttpRequest"/>
        /// </summary>
        /// <param name="context"><see cref="ClientInterceptorContext"/></param>
        /// <param name="request">grpc request</param>
        /// <returns><see cref="RequestLogModel"/></returns>
        private static RequestLogModel ExtractFromRequest<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return new RequestLogModel()
            {
                TimeUtc = DateTime.UtcNow,
                ContentType = "",
                Scheme = "",
                Method = context.Method.Name,
                Path = "",
                Headers = FormatHeaders(context.Options.Headers),
                QueryString = "",
                Body = request?.ToString() ?? string.Empty
            };
        }

        /// <summary>
        /// Convert request/response headers to Dictionary
        /// </summary>
        /// <param name="headers"><see cref="Metadata"/></param>
        /// <returns>Header in dictionary</returns>
        private static Dictionary<string, string> FormatHeaders(Metadata? headers)
        {
            var pairs = new Dictionary<string, string>();
            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    pairs.Add(header.Key, StringValues.IsNullOrEmpty(header.Value) ? "" : header.Value.ToString());
                }
            }
            return pairs;
        }
    }
}

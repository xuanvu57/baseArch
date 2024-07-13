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
    /// Interceptor to log the request and response for Grpc client
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/></param>
    /// <param name="dateTimeProvider"><see cref="IDateTimeProvider"/></param>
    public class GrpcClientLoggingInterceptor(ILogger<GrpcClientLoggingInterceptor> logger, IDateTimeProvider dateTimeProvider) : Interceptor
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

        /// <summary>
        /// Handle response to extract data
        /// </summary>
        /// <typeparam name="TRequest">Type of request</typeparam>
        /// <typeparam name="TResponse">Type of response</typeparam>
        /// <param name="request">Request</param>
        /// <param name="responseAsync">Response</param>
        /// <param name="context"><see cref="ClientInterceptorContext{TRequest, TResponse}"/></param>
        /// <returns>Response</returns>
        private async Task<TResponse> HandleResponse<TRequest, TResponse>(TRequest request, Task<TResponse> responseAsync, ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class
        {
            var requestLog = ExtractFromRequest(context, request);
            ResponseLogModel? responseLog = null;

            try
            {
                var response = await responseAsync;

                var responseBodyText = response?.ToString() ?? "";
                responseLog = ExtractFromResponse(context, StatusCode.OK, responseBodyText);

                return response!;
            }
            catch (RpcException ex)
            {
                responseLog = ExtractFromResponse(context, ex.StatusCode, ex.Message);
                throw;
            }
            finally
            {
                var requestResponseLogModel = new RequestResponseLogModel()
                {
                    RequestLogModel = requestLog,
                    ResponseLogModel = responseLog!
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
            logger.LogInformation(LogMessageTemplate.GrpcClientLogTemplate,
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
        private ResponseLogModel ExtractFromResponse<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, StatusCode statusCode, string responseBodyText)
            where TRequest : class
            where TResponse : class
        {
            return new ResponseLogModel()
            {
                ContentType = "",
                Header = FormatHeaders(context.Options.Headers),
                Body = responseBodyText,
                Status = statusCode.ToString(),
                TimeUtc = dateTimeProvider.GetUtcNow()
            };
        }

        /// <summary>
        /// Extract data from <see cref="HttpRequest"/>
        /// </summary>
        /// <param name="context"><see cref="ClientInterceptorContext"/></param>
        /// <param name="request">grpc request</param>
        /// <returns><see cref="RequestLogModel"/></returns>
        private RequestLogModel ExtractFromRequest<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, TRequest request)
            where TRequest : class
            where TResponse : class
        {
            return new RequestLogModel()
            {
                TimeUtc = dateTimeProvider.GetUtcNow(),
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

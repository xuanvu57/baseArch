using BaseArch.Presentation.RestApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace BaseArch.Presentation.RestApi.Middlewares
{
    /// <summary>
    /// Middleware to log data from <see cref="HttpRequest"/> and <see cref="HttpResponse"/>
    /// </summary>
    /// <param name="next"><see cref="RequestDelegate"/></param>
    public class HttpRequestResponseLoggingMiddleware(RequestDelegate next)
    {
        /// <summary>
        /// Handle the middleware
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <returns><see cref="Task"/></returns>
        public async Task Invoke(HttpContext httpContext, ILogger<HttpRequestResponseLoggingMiddleware> logger)
        {
            if (httpContext.Request.Path.ToString().Contains("swagger"))
            {
                await next(httpContext);
                return;
            }

            var request = await ExtractFromRequest(httpContext);

            var originalResponseBody = httpContext.Response.Body;
            using var newResponseBody = new MemoryStream();
            httpContext.Response.Body = newResponseBody;

            try
            {
                await next(httpContext);
            }
            finally
            {
                newResponseBody.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

                newResponseBody.Seek(0, SeekOrigin.Begin);
                await newResponseBody.CopyToAsync(originalResponseBody);

                var response = ExtractFromResponse(httpContext, responseBodyText);

                var requestResponseLogModel = new RequestResponseLogModel()
                {
                    RequestLogModel = request,
                    ResponseLogModel = response
                };

                WriteRequestResponseLog(logger, requestResponseLogModel);
            }
        }

        /// <summary>
        /// Write log
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="requestResponseLogModel"><see cref="RequestResponseLogModel"/></param>
        private static void WriteRequestResponseLog(ILogger<HttpRequestResponseLoggingMiddleware> logger, RequestResponseLogModel requestResponseLogModel)
        {
            logger.LogInformation("HTTP {RequestMethod} {RequestPath} responded {StatusCode} with {@RequestLogModel} {@ResponseLogModel}",
                requestResponseLogModel.RequestLogModel.Method,
                requestResponseLogModel.RequestLogModel.Path,
                requestResponseLogModel.ResponseLogModel.Status,
                requestResponseLogModel.RequestLogModel,
                requestResponseLogModel.ResponseLogModel);
        }

        /// <summary>
        /// Extract data from <see cref="HttpRequest"/>
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <returns><see cref="RequestLogModel"/></returns>
        private static async Task<RequestLogModel> ExtractFromRequest(HttpContext httpContext)
        {
            return new RequestLogModel()
            {
                TimeUtc = DateTime.UtcNow,
                ContentType = httpContext.Request.ContentType ?? "",
                Method = httpContext.Request.Method,
                Path = httpContext.Request.Path,
                Headers = FormatHeaders(httpContext.Request.Headers),
                QueryString = httpContext.Request.QueryString.ToString(),
                Body = await ReadBodyFromRequest(httpContext.Request)
            };
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

        /// <summary>
        /// Read body from <see cref="HttpRequest"/>
        /// </summary>
        /// <param name="request"><see cref="HttpRequest"/></param>
        /// <returns>Request body in string</returns>
        private static async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            request.EnableBuffering();
            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();
            request.Body.Position = 0;
            return requestBody;
        }
    }
}

using BaseArch.Application.Loggings;
using BaseArch.Application.Loggings.Models;
using BaseArch.Domain.Timezones.Interfaces;
using Microsoft.Extensions.Logging;

namespace BaseArch.Infrastructure.DefaultHttpClient.DelegatingHandlers
{
    public class HttpClientLoggingDelegatingHandler(ILogger<HttpClientLoggingDelegatingHandler> logger, IDateTimeProvider dateTimeProvider) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestLogModel = await ExtractFromRequest(request);
            HttpResponseMessage? response = null;
            try
            {
                response = await base.SendAsync(request, cancellationToken);
                response.EnsureSuccessStatusCode();
            }
            finally
            {
                var responseLogModel = await ExtractFromResponse(response!);
                var requestResponseLogModel = new RequestResponseLogModel()
                {
                    RequestLogModel = requestLogModel,
                    ResponseLogModel = responseLogModel
                };

                WriteRequestResponseLog(logger, requestResponseLogModel);
            }
            return response;
        }

        /// <summary>
        /// Write log
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/></param>
        /// <param name="requestResponseLogModel"><see cref="RequestResponseLogModel"/></param>
        private static void WriteRequestResponseLog(ILogger<HttpClientLoggingDelegatingHandler> logger, RequestResponseLogModel requestResponseLogModel)
        {
            logger.LogInformation(LogMessageTemplate.HttpClientLoggingDelegatingHandler,
                requestResponseLogModel.RequestLogModel.Method,
                requestResponseLogModel.RequestLogModel.Path,
                requestResponseLogModel.ResponseLogModel.Status,
                requestResponseLogModel.RequestLogModel,
                requestResponseLogModel.ResponseLogModel);
        }

        /// <summary>
        /// Extract data from <see cref="HttpRequestMessage"/>
        /// </summary>
        /// <param name="request"><see cref="HttpRequestMessage"/></param>
        /// <returns><see cref="RequestLogModel"/></returns>
        private async Task<RequestLogModel> ExtractFromRequest(HttpRequestMessage request)
        {
            return new RequestLogModel()
            {
                TimeUtc = dateTimeProvider.GetUtcNow(),
                ContentType = "",
                Scheme = request.RequestUri?.Scheme ?? "",
                Method = request.Method.Method,
                Path = request.RequestUri?.AbsolutePath ?? "",
                Headers = FormatHeaders(request.Headers.Where(header => header.Value.Any())),
                QueryString = request.RequestUri?.Query ?? "",
                Body = request.Content is null ? "" : await request.Content.ReadAsStringAsync()
            };
        }

        /// <summary>
        /// Extract data from <see cref="HttpResponseMessage"/>
        /// </summary>
        /// <param name="response"><see cref="HttpResponseMessage"/></param>
        /// <returns><see cref="ResponseLogModel"/></returns>
        private async Task<ResponseLogModel> ExtractFromResponse(HttpResponseMessage response)
        {
            return new ResponseLogModel()
            {
                ContentType = "",
                Header = FormatHeaders(response.Headers.Where(header => header.Value.Any())),
                Body = await response.Content.ReadAsStringAsync(),
                Status = response.StatusCode.ToString(),
                TimeUtc = dateTimeProvider.GetUtcNow(),
            };
        }

        /// <summary>
        /// Convert request headers to Dictionary
        /// </summary>
        /// <param name="headers">List of headers</param>
        /// <returns>Header in dictionary</returns>
        private static Dictionary<string, string> FormatHeaders(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            var pairs = new Dictionary<string, string>();
            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    pairs.Add(header.Key, string.Join(',', header.Value));
                }
            }
            return pairs;
        }
    }
}

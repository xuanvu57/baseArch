using BaseArch.Domain.Constants;
using BaseArch.Domain.Exceptions;
using BaseArch.Domain.StandardMessages;
using BaseArch.Domain.StandardMessages.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BaseArch.Application.ExceptionHandlers
{
    /// <summary>
    /// Default business excpetion handler
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/></param>
    /// <param name="standardMessageProvider"><see cref="IStandardMessageProvider"/></param>
    public class BusinessExceptionHandler(ILogger<BusinessExceptionHandler> logger, IStandardMessageProvider standardMessageProvider) : IExceptionHandler
    {
        /// <inheritdoc />
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var isHandled = false;
            switch (exception)
            {
                case BaseArchValidationException validationException:
                    await ValidationExceptionHandler(httpContext, validationException, cancellationToken).ConfigureAwait(false);
                    isHandled = true;
                    break;

                case ArgumentNullException argumentNullException:
                    await ArgumentNullExceptionHandler(httpContext, argumentNullException, cancellationToken).ConfigureAwait(false);
                    isHandled = true;
                    break;
            }

            return isHandled;
        }

        /// <summary>
        /// Handle the exception when argument is null
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContent"/></param>
        /// <param name="argumentNullException"><see cref="ArgumentNullException"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        private async Task ArgumentNullExceptionHandler(HttpContext httpContext, ArgumentNullException argumentNullException, CancellationToken cancellationToken)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Type = ProblemDetailsTypeConst.Type400BadRequest,
                Title = await standardMessageProvider.GetString(StandardMessagesConst.BAMSG0002, argumentNullException.ParamName ?? ""),
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }

        /// <summary>
        /// Handle the exception from validation
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/></param>
        /// <param name="validationException"><see cref="BaseArchValidationException"/></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        private async Task ValidationExceptionHandler(HttpContext httpContext, BaseArchValidationException validationException, CancellationToken cancellationToken)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Type = ProblemDetailsTypeConst.Type400BadRequest,
                Title = await standardMessageProvider.GetString(StandardMessagesConst.BAMSG0001),
                Status = StatusCodes.Status400BadRequest,
                Errors = validationException.Errors,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }
    }
}

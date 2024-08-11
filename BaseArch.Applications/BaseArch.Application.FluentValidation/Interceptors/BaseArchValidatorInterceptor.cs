using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.ErrorHandling;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BaseArch.Application.FluentValidation.Interceptors
{
    /// <summary>
    /// Base validator interceptor to handle if validation failed
    /// </summary>
    [DIService(DIServiceLifetime.Transient)]
    public class BaseArchValidatorInterceptor : IValidatorInterceptor
    {
        /// <inheritdoc/>
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                throw new BaseArchValidationException(result.ToDictionary());
            }

            return result;
        }

        /// <inheritdoc/>
        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }
    }
}

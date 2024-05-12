using BaseArch.Domain.Attributes;
using BaseArch.Domain.Enums;
using BaseArch.Domain.Exceptions;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BaseArch.Application.FluentValidation
{
    [DIService(DIServiceLifetime.Transient)]
    public class BaseArchValidatorInterceptor : IValidatorInterceptor
    {
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                throw new BaseArchValidationException(result.ToDictionary());
            }

            return result;
        }

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }
    }
}

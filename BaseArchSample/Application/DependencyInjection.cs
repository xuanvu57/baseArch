using BaseArch.Application.ExceptionHandlers;
using BaseArch.Application.Extensions;
using BaseArch.Application.FluentValidation.Extensions;
using BaseArch.Domain.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public class DependencyInjection : IDependencyInjection
    {
        public void Register(IServiceCollection services)
        {
            services.AddFluentValidators();

            services.AddBusinessExceptionHandler<BusinessExceptionHandler>();
        }
    }
}

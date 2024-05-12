using BaseArch.Application.ExceptionHandlers;
using BaseArch.Application.FluentValidation.Extensions;
using BaseArch.Domain.Interfaces;
using BaseArch.Infrastructure.Extensions;
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

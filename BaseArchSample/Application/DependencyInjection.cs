using BaseArch.Application.ExceptionHandlers;
using BaseArch.Application.Registrations;
using BaseArch.Application.FluentValidation.Registrations;
using BaseArch.Domain.DependencyInjection.Interfaces;
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

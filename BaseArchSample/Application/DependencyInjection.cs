using BaseArch.Application.ExceptionHandlers;
using BaseArch.Application.FluentValidation.Registrations;
using BaseArch.Application.ModuleRegistrations.Interfaces;
using BaseArch.Application.Registrations;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public class DependencyInjection : IModuleRegistration
    {
        public void Register(IServiceCollection services)
        {
            services.AddFluentValidators();

            services.AddBusinessExceptionHandler<BusinessExceptionHandler>();
        }
    }
}

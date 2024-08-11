using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Application.ModuleRegistrations.Interfaces
{
    /// <summary>
    /// Dependency injection contract
    /// </summary>
    public interface IModuleRegistration
    {
        /// <summary>
        /// Register the 3rd libraries
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        void Register(IServiceCollection services);
    }
}

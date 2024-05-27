using Microsoft.Extensions.DependencyInjection;

namespace BaseArch.Domain.DependencyInjection.Interfaces
{
    /// <summary>
    /// Dependency injection contract
    /// </summary>
    public interface IDependencyInjection
    {
        /// <summary>
        /// Register the 3rd libraries
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        void Register(IServiceCollection services);
    }
}

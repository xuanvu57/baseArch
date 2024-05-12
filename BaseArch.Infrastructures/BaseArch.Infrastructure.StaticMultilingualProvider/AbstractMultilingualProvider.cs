using BaseArch.Domain.Interfaces;
using Microsoft.Extensions.Localization;

namespace BaseArch.Infrastructure.StaticMultilingualProvider
{
    /// <summary>
    /// Abstract generic class for static multilingual provider
    /// </summary>
    /// <typeparam name="TResource">Type of resource class</typeparam>
    /// <param name="localizer"><see cref="IStringLocalizer"/></param>
    public abstract class AbstractMultilingualProvider<TResource>(IStringLocalizer<TResource> localizer) : IMultilingualProvider where TResource : class
    {
        /// <inheritdoc/>
        public Task<string> GetString(string stringId, params string[] values)
        {
            var localizedString = localizer.GetString(stringId, values);

            return Task.FromResult(localizedString.Value);
        }
    }
}

using BaseArch.Domain.BaseArchMessages.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Infrastructure.StaticMultilingualProvider;
using Domain.MultilingualProviders.Interfaces;
using Infrastructure.Resources.SampleMessages;
using Microsoft.Extensions.Localization;

namespace Infrastructure.MultilingualProviders
{
    [DIService(DIServiceLifetime.Singleton)]
    public class SampleMultilingualProvider(IStringLocalizer<Messages> localizer) : AbstractMultilingualProvider<Messages>(localizer), ISampleMultilingualProvider, IBaseArchMessageProvider
    {
    }
}

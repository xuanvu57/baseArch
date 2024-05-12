using BaseArch.Domain.Attributes;
using BaseArch.Domain.Enums;
using BaseArch.Domain.StandardMessages.Interfaces;
using BaseArch.Infrastructure.StaticMultilingualProvider;
using Domain.MultilingualProviders.Interfaces;
using Infrastructure.Resources.SampleMessages;
using Microsoft.Extensions.Localization;

namespace Infrastructure.MultilingualProviders
{
    [DIService(DIServiceLifetime.Singleton)]
    public class SampleMultilingualProvider(IStringLocalizer<Messages> localizer) : AbstractMultilingualProvider<Messages>(localizer), ISampleMultilingualProvider, IStandardMessageProvider
    {
    }
}

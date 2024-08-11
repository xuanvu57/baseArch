using ArchUnitNET.Domain;
using BaseArch.Domain.Attributes;
using BaseArch.Tests.ArchTests.Fixtures;

namespace BaseArch.Tests.ArchTests.Abstracts
{
    [Collection(nameof(AssembliesCollection))]
    [IgnoreNamingConvention]
    public abstract class BaseArchTests(AssembliesFixtures assembliesFixtures)
    {
        protected readonly Architecture architecture = assembliesFixtures.Architecture;
    }
}

using ArchUnitNET.Domain;
using BaseArch.Tests.ArchTests.Fixtures;

namespace BaseArch.Tests.ArchTests.Abstracts
{
    [Collection(nameof(AssembliesCollection))]
    public abstract class BaseArchTests(AssembliesFixtures assembliesFixtures)
    {
        protected readonly Architecture architecture = assembliesFixtures.Architecture;
    }
}

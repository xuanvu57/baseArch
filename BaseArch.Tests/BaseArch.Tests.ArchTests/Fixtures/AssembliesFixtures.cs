using ArchUnitNET.Domain;
using ArchUnitNET.Loader;

namespace BaseArch.Tests.ArchTests.Fixtures
{
    public sealed class AssembliesFixtures : IDisposable
    {
        public Architecture Architecture { get; init; }

        public AssembliesFixtures()
        {
            Architecture = new ArchLoader()
                        .LoadFilteredDirectory(AppDomain.CurrentDomain.BaseDirectory, "BaseArch.*.dll", SearchOption.TopDirectoryOnly)
                        .Build();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

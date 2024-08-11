using ArchUnitNET.xUnit;
using BaseArch.Tests.ArchTests.Abstracts;
using BaseArch.Tests.ArchTests.Fixtures;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace BaseArch.Tests.ArchTests
{
    public class AccessModifiersTests(AssembliesFixtures assembliesFixtures) : BaseArchTests(assembliesFixtures)
    {
        private const string _generatedFieldNameForEnumWithSuffix = "::value__";

        [Fact]
        public void PublicFields_ShouldNot_Exists()
        {
            // Arrange
            var rule = FieldMembers()
                .That()
                .FollowCustomPredicate(x =>
                    x.Visibility == ArchUnitNET.Domain.Visibility.Public &&
                    !x.IsStatic!.Value &&
                    x.Writability == ArchUnitNET.Domain.Writability.Writable, "")
                .And()
                .DoNotHaveFullNameContaining(_generatedFieldNameForEnumWithSuffix)
                .Should()
                .NotExist();

            // Assert
            rule.Check(architecture);
        }
    }
}

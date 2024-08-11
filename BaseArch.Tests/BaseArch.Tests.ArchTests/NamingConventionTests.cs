using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.xUnit;
using BaseArch.Domain.Attributes;
using BaseArch.Tests.ArchTests.Abstracts;
using BaseArch.Tests.ArchTests.Constants;
using BaseArch.Tests.ArchTests.Fixtures;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

// reference: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names
namespace BaseArch.Tests.ArchTests
{
    public class NamingConventionTests(AssembliesFixtures assembliesFixtures) : BaseArchTests(assembliesFixtures)
    {
        [Fact]
        public void Interface_ShouldBe_PascalCaseAndStartWithI()
        {
            var rule = Interfaces()
                .That()
                .DoNotHaveNameContaining("`")
                .Should()
                .HaveName(RegexPatterns.PascalCaseForInterface, true);

            rule.Check(architecture);
        }

        [Fact]
        public void GenericInterface_ShouldBe_PascalCaseAndStartWithI()
        {
            var rule = Interfaces()
                .That()
                .HaveNameContaining("`")
                .Should()
                .HaveName(RegexPatterns.PascalCaseForGenericInterface, true);

            rule.Check(architecture);
        }

        [Fact]
        public void Class_ShouldBe_PascalCase()
        {
            var rule = Classes()
                .That()
                .DoNotHaveNameContaining("`")
                .Should()
                .HaveName(RegexPatterns.PascalCase, true);

            rule.Check(architecture);
        }

        [Fact]
        public void GenericClass_ShouldBe_PascalCase()
        {
            var rule = Classes()
                .That()
                .HaveNameContaining("`")
                .Should()
                .HaveName(RegexPatterns.PascalCaseForGenericClass, true);

            rule.Check(architecture);
        }

        [Fact]
        public void Methods_ShouldBe_PascalCase()
        {
            var rule = MethodMembers()
                .That()
                .AreNoConstructors()
                .And()
                .DoNotHaveFullNameContaining("::")
                .Should()
                .HaveName(RegexPatterns.PascalCase, true);

            rule.Check(architecture);
        }

        [Fact]
        public void Properties_ShouldBe_PascalCase()
        {
            var rule = PropertyMembers()
                .Should()
                .HaveName(RegexPatterns.PascalCase, true);

            rule.Check(architecture);
        }

        [Fact]
        public void PrivateFields_ShouldBe_CamelCaseAndStartWithUnderscore()
        {
            var rule = FieldMembers()
                .That()
                .ArePrivate()
                .Should()
                .HaveName(RegexPatterns.CamelCaseAndStartWithUnderscore, true);

            rule.Check(architecture);
        }

        [Fact]
        public void PublicConstants_ShouldBe_PascalCase()
        {
            var ignoredClasses = architecture.Classes
                .Where(x => x.HasAttribute(typeof(IgnoreNamingConventionAttribute).FullName));

            var rule = FieldMembers()
                .That()
                .ArePublic()
                .And()
                .AreStatic()
                .And()
                .AreNotDeclaredIn(ignoredClasses)
                .Should()
                .HaveName(RegexPatterns.PascalCase, true);

            rule.Check(architecture);
        }
    }
}

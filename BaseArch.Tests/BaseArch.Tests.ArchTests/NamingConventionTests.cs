using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.xUnit;
using BaseArch.Domain.Attributes;
using BaseArch.Tests.ArchTests.Abstracts;
using BaseArch.Tests.ArchTests.Constants;
using BaseArch.Tests.ArchTests.Fixtures;
using System.Text.RegularExpressions;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

// reference: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names
namespace BaseArch.Tests.ArchTests
{
    public class NamingConventionTests : BaseArchTests
    {
        private readonly IEnumerable<ArchUnitNET.Domain.Class> _ignoredClasses;
        public NamingConventionTests(AssembliesFixtures assembliesFixtures) : base(assembliesFixtures)
        {
            _ignoredClasses = architecture.Classes
                .Where(x =>
                    x.HasAttribute(typeof(IgnoreNamingConventionAttribute).FullName) ||
                    x.BaseClass.HasAttribute(typeof(IgnoreNamingConventionAttribute).FullName));
        }

        [Fact]
        public void Interface_ShouldBe_PascalCaseAndStartWithI()
        {
            // Arrange
            var rule = Interfaces()
                .That()
                .FollowCustomPredicate(x => !x.IsGeneric, "is not generic interface")
                .Should()
                .HaveName(RegexPatterns.PascalCaseForInterface, true);

            // Assert
            rule.Check(architecture);
        }

        [Fact]
        public void GenericInterface_ShouldBe_PascalCaseAndStartWithI()
        {
            // Arrange
            var rule = Interfaces()
                .That()
                .FollowCustomPredicate(x => x.IsGeneric, "is generic interface")
                .Should()
                .HaveName(RegexPatterns.PascalCaseForGenericInterface, true);

            // Assert
            rule.Check(architecture);
        }

        [Fact]
        public void Class_ShouldBe_PascalCase()
        {
            // Arrange
            var rule = Classes()
                .That()
                .FollowCustomPredicate(x => !x.IsGeneric, "is not generic class")
                .Should()
                .HaveName(RegexPatterns.PascalCase, true);

            // Assert
            rule.Check(architecture);
        }

        [Fact]
        public void GenericClass_ShouldBe_PascalCase()
        {
            // Arrange
            var rule = Classes()
                .That()
                .FollowCustomPredicate(x => x.IsGeneric, "is generic class")
                .Should()
                .HaveName(RegexPatterns.PascalCaseForGenericClass, true);

            // Assert
            rule.Check(architecture);
        }

        [Fact]
        public void Methods_ShouldBe_PascalCase()
        {
            // Arrange
            var rule = MethodMembers()
                .That()
                .FollowCustomPredicate(x =>
                    x.MethodForm == ArchUnitNET.Domain.MethodForm.Normal &&
                    !x.Attributes.Any(a => a.Name == typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute).Name) &&
                    !_ignoredClasses.Contains(x.DeclaringType) &&
                    !Regex.IsMatch(x.Name.Substring(0, x.Name.IndexOf('(')), RegexPatterns.PascalCase), "name is not matched to Pascal case")
                .Should()
                .NotExist();

            // Assert
            rule.Check(architecture);
        }

        [Fact]
        public void Properties_ShouldBe_PascalCase()
        {
            // Arrange
            var rule = PropertyMembers()
                .Should()
                .HaveName(RegexPatterns.PascalCase, true);

            // Assert
            rule.Check(architecture);
        }

        [Fact]
        public void PrivateFields_ShouldBe_CamelCaseAndStartWithUnderscore()
        {
            // Arrange
            var rule = FieldMembers()
                .That()
                .ArePrivate()
                .Should()
                .HaveName(RegexPatterns.CamelCaseAndStartWithUnderscore, true);

            // Assert
            rule.Check(architecture);
        }

        [Fact]
        public void PublicConstants_ShouldBe_PascalCase()
        {
            // Arrange
            var rule = FieldMembers()
                .That()
                .ArePublic()
                .And()
                .AreStatic()
                .And()
                .AreNotDeclaredIn(_ignoredClasses)
                .Should()
                .HaveName(RegexPatterns.PascalCase, true);

            // Assert
            rule.Check(architecture);
        }
    }
}

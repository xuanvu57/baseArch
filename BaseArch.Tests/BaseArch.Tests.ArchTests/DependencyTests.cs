using ArchUnitNET.xUnit;
using BaseArch.Domain.AssemblyLayer;
using BaseArch.Tests.ArchTests.Abstracts;
using BaseArch.Tests.ArchTests.Fixtures;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace BaseArch.Tests.ArchTests
{
    public class DependencyTests(AssembliesFixtures assembliesFixtures) : BaseArchTests(assembliesFixtures)
    {
        [Fact]
        public void DomainLayer_ShouldNot_DependOnOtherLayers()
        {
            // Arrange
            var domainAssemblies = architecture.Assemblies
                .Where(x => x.AttributeInstances.Exists(attribute =>
                    attribute.Type.Name == typeof(LayerInfoAttribute).Name &&
                    attribute.AttributeArguments.Any(arg =>
                        (CleanArchitectureLayerTypes)arg.Value == CleanArchitectureLayerTypes.Domain)));

            if (domainAssemblies.Any())
            {
                foreach (var assembly in domainAssemblies)
                {
                    var typesFromOtherAssemblies = architecture.Types
                        .Where(type => type.Assembly != assembly);
                    var classesFromOtherAssemblies = architecture.Classes
                        .Where(cls => cls.Assembly != assembly);

                    var ruleForType = Types()
                        .That()
                        .ResideInAssembly(assembly)
                        .Should()
                        .NotDependOnAny(typesFromOtherAssemblies);

                    var ruleForClass = Classes()
                        .That()
                        .ResideInAssembly(assembly)
                        .Should()
                        .NotDependOnAny(classesFromOtherAssemblies);

                    // Assert
                    ruleForType.Check(architecture);
                    ruleForClass.Check(architecture);
                }
            }

            Assert.True(true);
        }

        [Fact]
        public void ApplicationLayer_Should_OnlyDependOnInnerLayers()
        {
            // Arrange
            var innerLayersAssemblies = architecture.Assemblies
                .Where(x => x.AttributeInstances.Exists(attribute =>
                    attribute.Type.Name == typeof(LayerInfoAttribute).Name &&
                    attribute.AttributeArguments.Any(arg =>
                        (CleanArchitectureLayerTypes)arg.Value == CleanArchitectureLayerTypes.Domain)));
            var applicationAssemblies = architecture.Assemblies
                .Where(x => x.AttributeInstances.Exists(attribute =>
                    attribute.Type.Name == typeof(LayerInfoAttribute).Name &&
                    attribute.AttributeArguments.Any(arg =>
                        (CleanArchitectureLayerTypes)arg.Value == CleanArchitectureLayerTypes.Application)));

            if (applicationAssemblies.Any())
            {
                foreach (var assembly in applicationAssemblies)
                {
                    var typesFromInnerAssemblies = architecture.Types
                        .Where(type =>
                            type.Assembly != assembly &&
                            !innerLayersAssemblies.Any(x => x == type.Assembly));

                    var classesFromInnerAssemblies = architecture.Classes
                        .Where(cls =>
                            cls.Assembly != assembly &&
                            !innerLayersAssemblies.Any(x => x == cls.Assembly));

                    var ruleForType = Types()
                        .That()
                        .ResideInAssembly(assembly)
                        .Should()
                        .NotDependOnAny(typesFromInnerAssemblies);

                    var ruleForClass = Classes()
                        .That()
                        .ResideInAssembly(assembly)
                        .Should()
                        .NotDependOnAny(classesFromInnerAssemblies);

                    // Assert
                    ruleForType.Check(architecture);
                    ruleForClass.Check(architecture);
                }
            }

            Assert.True(true);
        }

        [Fact]
        public void InfrastructureLayer_Should_OnlyDependOnInnerLayer()
        {
            // Arrange
            var innerLayersAssemblies = architecture.Assemblies
                .Where(x => x.AttributeInstances.Exists(attribute =>
                    attribute.Type.Name == typeof(LayerInfoAttribute).Name &&
                    attribute.AttributeArguments.Any(arg =>
                        (CleanArchitectureLayerTypes)arg.Value == CleanArchitectureLayerTypes.Domain ||
                        (CleanArchitectureLayerTypes)arg.Value == CleanArchitectureLayerTypes.Application)));
            var infrastructureAssemblies = architecture.Assemblies
                .Where(x => x.AttributeInstances.Exists(attribute =>
                    attribute.Type.Name == typeof(LayerInfoAttribute).Name &&
                    attribute.AttributeArguments.Any(arg =>
                        (CleanArchitectureLayerTypes)arg.Value == CleanArchitectureLayerTypes.Infrastructure)));

            if (infrastructureAssemblies.Any())
            {
                foreach (var assembly in infrastructureAssemblies)
                {
                    var typesFromInnerAssemblies = architecture.Types
                        .Where(type =>
                            type.Assembly != assembly &&
                            !innerLayersAssemblies.Any(x => x == type.Assembly));

                    var classesFromInnerAssemblies = architecture.Classes
                        .Where(cls =>
                            cls.Assembly != assembly &&
                            !innerLayersAssemblies.Any(x => x == cls.Assembly));

                    var ruleForType = Types()
                        .That()
                        .ResideInAssembly(assembly)
                        .Should()
                        .NotDependOnAny(typesFromInnerAssemblies);


                    var ruleForClass = Classes()
                        .That()
                        .ResideInAssembly(assembly)
                        .Should()
                        .NotDependOnAny(classesFromInnerAssemblies);

                    // Assert
                    ruleForType.Check(architecture);
                    ruleForClass.Check(architecture);
                }
            }

            Assert.True(true);
        }
    }
}

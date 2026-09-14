using CSharpFunctionalExtensions.HttpResults.Generators.Models;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Builders;

internal sealed class UnitResultExtensionsClassBuilder(IReadOnlyList<MapperDescriptor> mapperDescriptors)
  : ExtensionClassBuilder(mapperDescriptors)
{
  protected override string ClassName => "UnitResultExtensions";

  protected override string ClassSummary =>
    """
      /// <summary>
      /// Extension methods <see cref="UnitResult{E}"/>
      /// </summary>
      """;

  internal override IReadOnlyList<HttpResultMethodDefinition> Methods =>
    HttpResultMethodCatalog.UnitResultWithCustomErrorMethods;
}

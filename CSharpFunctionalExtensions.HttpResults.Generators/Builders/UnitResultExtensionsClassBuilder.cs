using CSharpFunctionalExtensions.HttpResults.Generators.Utils;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Builders;

internal sealed class UnitResultExtensionsClassBuilder(IReadOnlyList<MapperInfo> mapperInfos)
  : ClassBuilder(mapperInfos)
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

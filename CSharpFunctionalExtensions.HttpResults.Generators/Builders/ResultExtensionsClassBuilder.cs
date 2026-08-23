using CSharpFunctionalExtensions.HttpResults.Generators.Utils;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Builders;

internal sealed class ResultExtensionsClassBuilder(IReadOnlyList<MapperInfo> mapperInfos) : ClassBuilder(mapperInfos)
{
  protected override string ClassName => "ResultExtensions";

  protected override string ClassSummary =>
    """
      /// <summary>
      /// Extension methods for <see cref="Result{T,E}"/>
      /// </summary>
      """;

  internal override IReadOnlyList<HttpResultMethodDefinition> Methods =>
    HttpResultMethodCatalog.ResultWithCustomErrorMethods;
}

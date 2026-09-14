using CSharpFunctionalExtensions.HttpResults.Generators.Models;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Builders;

internal sealed class ResultExtensionsClassBuilder(IReadOnlyList<MapperDescriptor> mapperDescriptors)
  : ExtensionClassBuilder(mapperDescriptors)
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

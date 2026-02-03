using CSharpFunctionalExtensions.HttpResults.Generators.UnitResultExtensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Builders;

public class UnitResultExtensionsClassBuilder(
  List<ClassDeclarationSyntax> mapperClasses,
  Compilation? compilation = null
) : ClassBuilder(mapperClasses, compilation)
{
  protected override string ClassName => "UnitResultExtensions";

  protected override string ClassSummary =>
    """
      /// <summary>
      /// Extension methods <see cref="UnitResult{E}"/>
      /// </summary>
      """;

  internal override List<IGenerateMethods> MethodGenerators =>
    [new ToStatusCodeHttpResultE(), new ToNoContentHttpResultE()];
}

using CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Builders;

public class ResultExtensionsClassBuilder(
  List<ClassDeclarationSyntax> mapperClasses,
  Compilation? compilation = null
) : ClassBuilder(mapperClasses, compilation)
{
  protected override string ClassName => "ResultExtensions";

  protected override string ClassSummary =>
    """
      /// <summary>
      /// Extension methods for <see cref="Result{T,E}"/>
      /// </summary>
      """;

  internal override List<IGenerateMethods> MethodGenerators =>
    [
      new ToAcceptedAtRouteHttpResultTE(),
      new ToAcceptedHttpResultTE(),
      new ToCreatedAtRouteHttpResultTE(),
      new ToCreatedHttpResultTE(),
      new ToFileHttpResultByteArrayE(),
      new ToFileStreamHttpResultStreamE(),
      new ToJsonHttpResultTE(),
      new ToNoContentHttpResultTE(),
      new ToStatusCodeHttpResultTE(),
      new ToOkHttpResultTE(),
      new ToContentHttpResultStringE(),
      new ToServerSentEventsHttpResultIAsyncEnumerableTE(),
    ];
}

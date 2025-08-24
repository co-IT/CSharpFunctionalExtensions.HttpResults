using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Rules;

internal class ParameterlessConstructorRule : IRule
{
  public DiagnosticDescriptor RuleDescriptor { get; } =
    new(
      "CFEHTTPR004",
      "Missing parameterless constructor in IResultErrorMapper",
      "Class '{0}' does not have a parameterless constructor",
      "Mapping",
      DiagnosticSeverity.Error,
      true,
      customTags: ["CompilationEnd"]
    );

  public IEnumerable<Diagnostic> Check(List<ClassDeclarationSyntax> mapperClasses)
  {
    foreach (var mapperClass in mapperClasses)
    {
      if (!HasParameterlessConstructor(mapperClass))
        yield return Diagnostic.Create(
          RuleDescriptor,
          mapperClass.Identifier.GetLocation(),
          mapperClass.Identifier.Text
        );
    }
  }

  private static bool HasParameterlessConstructor(ClassDeclarationSyntax classDeclaration)
  {
    var hasExplicitParameterless = classDeclaration
      .Members.OfType<ConstructorDeclarationSyntax>()
      .Any(c => c.ParameterList.Parameters.Count == 0);

    if (hasExplicitParameterless)
      return true;

    var hasAnyConstructors = classDeclaration.Members.OfType<ConstructorDeclarationSyntax>().Any();

    return !hasAnyConstructors;
  }
}

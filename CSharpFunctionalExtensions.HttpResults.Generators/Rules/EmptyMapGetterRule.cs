using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Rules;

internal class EmptyMapGetterRule : IRule
{
  private const string ErrorResultMapFunc = "Map";

  public DiagnosticDescriptor RuleDescriptor { get; } =
    new(
      "CFEHTTPR003",
      "Empty Map getter in IResultErrorMapper",
      "Class '{0}' must implement the 'Map' property getter",
      "Mapping",
      DiagnosticSeverity.Error,
      true,
      customTags: ["CompilationEnd"]
    );

  public IEnumerable<Diagnostic> Check(List<ClassDeclarationSyntax> mapperClasses)
  {
    foreach (var mapperClass in mapperClasses)
    {
      var mappingProperty = mapperClass
        .Members.OfType<PropertyDeclarationSyntax>()
        .FirstOrDefault(member => member.Identifier.Text == ErrorResultMapFunc);

      if (mappingProperty == null)
        continue;

      // Check if the property has an expression-bodied getter (e.g., `Map => error => ...`)
      if (mappingProperty.ExpressionBody != null)
        continue; // Valid implementation

      var getter = mappingProperty.AccessorList?.Accessors.FirstOrDefault(accessor =>
        accessor.IsKind(SyntaxKind.GetAccessorDeclaration)
      );

      // Check if the getter is an auto-property (e.g., `get;`)
      if (getter?.Body == null && getter?.ExpressionBody == null)
        yield return Diagnostic.Create(RuleDescriptor, mappingProperty.GetLocation(), mapperClass.Identifier.Text);
    }
  }
}

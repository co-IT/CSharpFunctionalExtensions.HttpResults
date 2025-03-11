using CSharpFunctionalExtensions.HttpResults.Generators.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.HttpResults.Generators;

/// <summary>
///   Validates the rules for the <see cref="ResultExtensionsGenerator" />.
/// </summary>
internal static class ResultExtensionsGeneratorValidator
{
  private static readonly List<IRule> Rules = [new DuplicateMapperRule()];

  /// <summary>
  ///   Validates the rules for the generator.
  /// </summary>
  /// <param name="mapperClasses">The list of mapper classes to validate.</param>
  /// <param name="context">The source production context for reporting diagnostics.</param>
  /// <returns>True if all rules are satisfied; otherwise, false.</returns>
  public static bool CheckRules(List<ClassDeclarationSyntax> mapperClasses, SourceProductionContext context)
  {
    var diagnostics = new List<Diagnostic>();

    foreach (var rule in Rules)
      diagnostics.AddRange(rule.Check(mapperClasses));

    foreach (var diagnostic in diagnostics)
      context.ReportDiagnostic(diagnostic);

    return !diagnostics.Any();
  }
}

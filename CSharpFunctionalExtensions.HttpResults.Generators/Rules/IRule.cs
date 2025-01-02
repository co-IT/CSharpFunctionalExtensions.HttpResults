using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Rules;

/// <summary>
///   Defines a rule for validating mapper classes.
/// </summary>
internal interface IRule
{
  /// <summary>
  ///   Gets the diagnostic descriptor for the rule.
  /// </summary>
  DiagnosticDescriptor RuleDescriptor { get; }

  /// <summary>
  ///   Checks the rule against a list of mapper classes.
  /// </summary>
  /// <param name="mapperClasses">The list of mapper classes to validate.</param>
  /// <returns>A collection of diagnostics representing rule violations.</returns>
  IEnumerable<Diagnostic> Check(List<ClassDeclarationSyntax> mapperClasses);
}

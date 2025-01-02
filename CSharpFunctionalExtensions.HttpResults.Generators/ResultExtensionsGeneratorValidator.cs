using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.HttpResults.Generators;

internal static class ResultExtensionsGeneratorValidator
{
  private const string ErrorResultMapFunc = "Map";

  private static readonly DiagnosticDescriptor DuplicateMapperRule = new(
    "CFEHTTPR002",
    "Duplicate ResultErrorMapper",
    "Class '{0}' does have multiple IResultErrorMapper",
    "Mapping",
    DiagnosticSeverity.Error,
    true,
    customTags: ["CompilationEnd"]
  );

  public static bool CheckRules(List<ClassDeclarationSyntax> mapperClasses, SourceProductionContext context)
  {
    var diagnostics = CheckForDuplicateMappers(mapperClasses).ToList();

    foreach (var diagnostic in diagnostics)
      context.ReportDiagnostic(diagnostic);

    return !diagnostics.Any();
  }

  private static IEnumerable<Diagnostic> CheckForDuplicateMappers(List<ClassDeclarationSyntax> mapperClasses)
  {
    var mappedResultErrorTypes = mapperClasses
      .Select(mapperClass =>
      {
        var mappingProperty =
          mapperClass.Members.FirstOrDefault(member =>
            (member as PropertyDeclarationSyntax)?.Identifier.Text == ErrorResultMapFunc
          ) as PropertyDeclarationSyntax;
        var mappingTypes = (mappingProperty?.Type as GenericNameSyntax)?.TypeArgumentList.Arguments.ToArray();

        return mappingTypes?[0];
      })
      .Where(type => type != null)
      .ToList();

    var duplicateMappedResultErrorClassNames = mappedResultErrorTypes
      .GroupBy(type => type!.ToString())
      .Where(grouping => grouping.Count() > 1)
      .Select(grouping => grouping.Key)
      .ToList();

    foreach (var duplicateMappedResultErrorClassName in duplicateMappedResultErrorClassNames)
    {
      var location = mapperClasses
        .Select(mapperClass =>
        {
          var mappingProperty =
            mapperClass.Members.FirstOrDefault(member =>
              (member as PropertyDeclarationSyntax)?.Identifier.Text == ErrorResultMapFunc
            ) as PropertyDeclarationSyntax;
          var mappingTypes = (mappingProperty?.Type as GenericNameSyntax)?.TypeArgumentList.Arguments.ToArray();

          return mappingTypes?[0];
        })
        .Last(typeSyntax => typeSyntax!.ToString() == duplicateMappedResultErrorClassName)
        ?.GetLocation();

      yield return Diagnostic.Create(DuplicateMapperRule, location, duplicateMappedResultErrorClassName);
    }
  }
}

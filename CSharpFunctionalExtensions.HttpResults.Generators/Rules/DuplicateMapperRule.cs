using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Rules;

internal class DuplicateMapperRule : IRule
{
  private const string ErrorResultMapFunc = "Map";

  public DiagnosticDescriptor RuleDescriptor { get; } =
    new(
      "CFEHTTPR002",
      "Duplicate ResultErrorMapper",
      "Class '{0}' does have multiple IResultErrorMapper",
      "Mapping",
      DiagnosticSeverity.Error,
      true,
      customTags: ["CompilationEnd"]
    );

  public IEnumerable<Diagnostic> Check(List<ClassDeclarationSyntax> mapperClasses)
  {
    var mappedResultErrorTypes = GetMappedResultErrorTypes(mapperClasses);
    var duplicateMappedResultErrorClassNames = GetDuplicateMappedResultErrorClassNames(mappedResultErrorTypes);

    foreach (var duplicateClassName in duplicateMappedResultErrorClassNames)
    {
      var location = GetLocationOfDuplicate(mapperClasses, duplicateClassName);
      yield return Diagnostic.Create(RuleDescriptor, location, duplicateClassName);
    }
  }

  private static List<TypeSyntax> GetMappedResultErrorTypes(List<ClassDeclarationSyntax> mapperClasses)
  {
    return mapperClasses.Select(GetMappedResultErrorType).Where(type => type != null).ToList()!;
  }

  private static TypeSyntax? GetMappedResultErrorType(ClassDeclarationSyntax mapperClass)
  {
    var mappingProperty = mapperClass
      .Members.OfType<PropertyDeclarationSyntax>()
      .FirstOrDefault(member => member.Identifier.Text == ErrorResultMapFunc);
    return (mappingProperty?.Type as GenericNameSyntax)?.TypeArgumentList.Arguments.FirstOrDefault();
  }

  private static List<string> GetDuplicateMappedResultErrorClassNames(List<TypeSyntax> mappedResultErrorTypes)
  {
    return mappedResultErrorTypes
      .GroupBy(type => type!.ToString())
      .Where(grouping => grouping.Count() > 1)
      .Select(grouping => grouping.Key)
      .ToList();
  }

  private static Location? GetLocationOfDuplicate(List<ClassDeclarationSyntax> mapperClasses, string duplicateClassName)
  {
    return mapperClasses
      .Select(GetMappedResultErrorType)
      .Last(typeSyntax => typeSyntax!.ToString() == duplicateClassName)
      ?.GetLocation();
  }
}

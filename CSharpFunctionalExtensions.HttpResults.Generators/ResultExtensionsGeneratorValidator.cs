using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.HttpResults.Generators;

public class ResultExtensionsGeneratorValidator
{
    private static readonly DiagnosticDescriptor MissingMapperRule = new(
        "CFEMAPI001",
        "Missing ResultErrorMapper",
        "Class '{0}' does not have a corresponding IResultErrorMapper",
        "Mapping",
        DiagnosticSeverity.Error,
        true,
        customTags: ["CompilationEnd"]);
    
    private static readonly DiagnosticDescriptor DuplicateMapperRule = new(
        "CFEMAPI002",
        "Duplicate ResultErrorMapper",
        "Class '{0}' does have multiple IResultErrorMapper",
        "Mapping",
        DiagnosticSeverity.Error,
        true,
        customTags: ["CompilationEnd"]);
    
    public static bool CheckRules(List<ClassDeclarationSyntax> mapperClasses, List<ClassDeclarationSyntax> resultErrorClasses, GeneratorExecutionContext context)
    {
        var diagnostics = CheckForMissingMappers(mapperClasses, resultErrorClasses)
            .Concat(CheckForDuplicateMappers(mapperClasses, resultErrorClasses))
            .ToList();
        
        foreach (var diagnostic in diagnostics)
            context.ReportDiagnostic(diagnostic);
        
        return !diagnostics.Any();
    }
    
    
    private static IEnumerable<Diagnostic> CheckForMissingMappers(List<ClassDeclarationSyntax> mapperClasses, List<ClassDeclarationSyntax> resultErrorClasses)
    {
        var mappedResultErrorClassNames = mapperClasses
            .Select(mapperClass =>
            {
                var mappingProperty = mapperClass.Members
                    .FirstOrDefault(member => (member as PropertyDeclarationSyntax)?.Identifier.Text == "Map") as PropertyDeclarationSyntax;
                var mappingTypes = (mappingProperty?.Type as GenericNameSyntax)?.TypeArgumentList.Arguments
                    .Select(type => type.ToString())
                    .ToArray();
                
                return mappingTypes![0];
            })
            .Distinct()
            .ToList();
        
        var resultErrorClassNames = resultErrorClasses
            .Select(classDeclaration => classDeclaration.Identifier.Text)
            .Distinct()
            .ToList();
        
        var notMappedResultErrorClassNames = resultErrorClassNames.Except(mappedResultErrorClassNames);
        
        foreach (var notMappedResultErrorClassName in notMappedResultErrorClassNames)
        {
            var resultErrorClass = resultErrorClasses
                .First(classDeclaration => classDeclaration.Identifier.Text == notMappedResultErrorClassName);
            
            yield return Diagnostic.Create(MissingMapperRule, resultErrorClass.GetLocation(), notMappedResultErrorClassName);
        }
    }
    
    private static IEnumerable<Diagnostic> CheckForDuplicateMappers(List<ClassDeclarationSyntax> mapperClasses, List<ClassDeclarationSyntax> resultErrorClasses)
    {
        var mappedResultErrorClassNames = mapperClasses.Select(mapperClass =>
            {
                var mappingProperty = mapperClass.Members
                    .FirstOrDefault(member => (member as PropertyDeclarationSyntax)?.Identifier.Text == "Map") as PropertyDeclarationSyntax;
                var mappingTypes = (mappingProperty?.Type as GenericNameSyntax)?.TypeArgumentList.Arguments
                    .Select(type => type.ToString())
                    .ToArray();
                
                return mappingTypes![0];
            })
            .ToList();
        
        var duplicateMappedResultErrorClassNames = mappedResultErrorClassNames
            .GroupBy(className => className)
            .Where(grouping => grouping.Count() > 1)
            .Select(grouping => grouping.Key)
            .ToList();
        
        foreach (var duplicateMappedResultErrorClassName in duplicateMappedResultErrorClassNames)
        {
            var resultErrorClass = resultErrorClasses
                .First(classDeclaration => classDeclaration.Identifier.Text == duplicateMappedResultErrorClassName);
            
            yield return Diagnostic.Create(DuplicateMapperRule, resultErrorClass.GetLocation(), duplicateMappedResultErrorClassName);
        }
    }
}
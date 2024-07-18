using CSharpFunctionalExtensions.HttpResults.Generators.UnitResultExtensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Builders;

public class UnitResultExtensionsClassBuilder(List<string> requiredNamespaces, List<ClassDeclarationSyntax> mapperClasses) : ClassBuilder(requiredNamespaces, mapperClasses)
{
    protected override string ClassName => "UnitResultExtensions";
    
    protected override string ClassSummary => """
                                              /// <summary>
                                              /// Extension methods <see cref="UnitResult{E}"/>
                                              /// </summary>
                                              """;
    
    internal override List<IGenerateMethods> MethodGenerators => [new ToStatusCodeHttpResultE()];
}
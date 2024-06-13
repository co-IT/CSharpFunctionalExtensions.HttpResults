using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CSharpFunctionalExtensions.MinimalApi.Generators;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ResultErrorMapperValidator : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor Rule = new(
        "XXXX",
        "Missing ResultErrorMapper",
        "Class '{0}' does not have a IResultErrorMapper",
        "Mapping",
        DiagnosticSeverity.Error,
        true);
    
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];
    
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ResultSyntaxReceiver());
    }
    
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(compilationStartContext =>
        {
            var resultErrorClasses = new List<ClassDeclarationSyntax>();
            var mapperClasses = new List<ClassDeclarationSyntax>();
            var receiver = new ResultSyntaxReceiver();
            
            compilationStartContext.RegisterSyntaxNodeAction(context =>
            {
                receiver.OnVisitSyntaxNode(context.Node);
                
                resultErrorClasses.AddRange(receiver.ResultErrorClasses);
                mapperClasses.AddRange(receiver.MapperClasses);
            }, SyntaxKind.ClassDeclaration);
            
            compilationStartContext.RegisterCompilationEndAction(compilationEndContext =>
            {
                var resultClassesWithoutMapper = resultErrorClasses.Where(@class => !mapperClasses.Any(x => ((x.BaseList.Types.First().Type as GenericNameSyntax).TypeArgumentList.Arguments.First() as IdentifierNameSyntax).Identifier.Text == @class.Identifier.Text));
                
                // if (resultErrorClasses != mapperClasses)
                // {
                //     var diagnostic = Diagnostic.Create(Rule, null);
                //     compilationEndContext.ReportDiagnostic(diagnostic);
                // }
            });
        });
    }
}
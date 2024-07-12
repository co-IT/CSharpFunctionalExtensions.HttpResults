using System.Text;
using CSharpFunctionalExtensions.HttpResults.Generators.Builders;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CSharpFunctionalExtensions.HttpResults.Generators;

[Generator]
internal class ResultExtensionsGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ResultSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not ResultSyntaxReceiver receiver)
            return;

        ResultExtensionsGeneratorValidator.CheckRules(receiver.MapperClasses, context);

        var classBuilders = new List<ClassBuilder>
        {
            new ResultExtensionsClassBuilder(receiver.RequiredNamespaces, receiver.MapperClasses),
            new UnitResultExtensionsClassBuilder(receiver.RequiredNamespaces, receiver.MapperClasses)
        };

        foreach (var classBuilder in classBuilders)
            context.AddSource(classBuilder.SourceFileName, SourceText.From(classBuilder.Build(), Encoding.UTF8));
    }
}
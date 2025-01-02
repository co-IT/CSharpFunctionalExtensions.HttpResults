using System.Text;
using CSharpFunctionalExtensions.HttpResults.Generators.Builders;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CSharpFunctionalExtensions.HttpResults.Generators;

[Generator]
internal class ResultExtensionsGenerator : IIncrementalGenerator
{
  private const string ResultErrorMapperInterface = "IResultErrorMapper";

  public void Initialize(IncrementalGeneratorInitializationContext context)
  {
    var classDeclarations = context
      .SyntaxProvider.CreateSyntaxProvider(
        predicate: static (node, _) => node is ClassDeclarationSyntax,
        transform: static (context, _) => (ClassDeclarationSyntax)context.Node
      )
      .Where(static classDeclaration =>
        classDeclaration.BaseList?.Types.Any(baseType =>
          baseType.Type is GenericNameSyntax nameSyntax
          && nameSyntax.Identifier.Text.StartsWith(ResultErrorMapperInterface)
        ) == true
      );

    var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

    context.RegisterSourceOutput(
      compilationAndClasses,
      static (context, source) =>
      {
        var (_, classDeclarations) = source;

        var mapperClasses = new List<ClassDeclarationSyntax>();
        var requiredNamespaces = new List<string>();

        foreach (var classDeclaration in classDeclarations)
        {
          mapperClasses.Add(classDeclaration);
          requiredNamespaces.Add(GetNamespace(classDeclaration));
        }

        if (!ResultExtensionsGeneratorValidator.CheckRules(mapperClasses, context))
          return;

        var classBuilders = new List<ClassBuilder>
        {
          new ResultExtensionsClassBuilder(requiredNamespaces, mapperClasses),
          new UnitResultExtensionsClassBuilder(requiredNamespaces, mapperClasses),
        };

        foreach (var classBuilder in classBuilders)
        {
          context.AddSource(classBuilder.SourceFileName, SourceText.From(classBuilder.Build(), Encoding.UTF8));
        }
      }
    );
  }

  private static string GetNamespace(ClassDeclarationSyntax classDeclaration)
  {
    var namespaceDeclaration = classDeclaration.Parent;

    while (
      namespaceDeclaration != null
      && namespaceDeclaration is not NamespaceDeclarationSyntax
      && namespaceDeclaration is not FileScopedNamespaceDeclarationSyntax
    )
      namespaceDeclaration = namespaceDeclaration.Parent;

    return namespaceDeclaration switch
    {
      NamespaceDeclarationSyntax namespaceSyntax => namespaceSyntax.Name.ToString(),
      FileScopedNamespaceDeclarationSyntax fileScopedNamespaceSyntax => fileScopedNamespaceSyntax.Name.ToString(),
      _ => string.Empty,
    };
  }
}

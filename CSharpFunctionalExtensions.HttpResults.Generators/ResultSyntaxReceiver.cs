using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.HttpResults.Generators;

internal class ResultSyntaxReceiver : ISyntaxReceiver
{
  public List<ClassDeclarationSyntax> MapperClasses { get; } = new();
  public List<string> RequiredNamespaces { get; } = new();

  public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
  {
    // Look for class declarations that implement the specified interface
    if (syntaxNode is not ClassDeclarationSyntax classDeclaration)
      return;

    foreach (var baseType in classDeclaration.BaseList?.Types ?? [])
      if (
        baseType.Type is GenericNameSyntax nameSyntax
        && classDeclaration is not null
        && nameSyntax.Identifier.Text.StartsWith("IResultErrorMapper")
      )
      {
        MapperClasses.Add(classDeclaration);
        RequiredNamespaces.Add(GetNamespace(classDeclaration));
        break;
      }
  }

  private static string GetNamespace(ClassDeclarationSyntax classDeclaration)
  {
    var namespaceDeclaration = classDeclaration.Parent;

    while (
      namespaceDeclaration != null
      && !(namespaceDeclaration is NamespaceDeclarationSyntax)
      && !(namespaceDeclaration is FileScopedNamespaceDeclarationSyntax)
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

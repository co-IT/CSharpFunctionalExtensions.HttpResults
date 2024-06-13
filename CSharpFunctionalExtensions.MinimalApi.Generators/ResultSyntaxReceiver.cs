using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpFunctionalExtensions.MinimalApi.Generators;

public class ResultSyntaxReceiver : ISyntaxReceiver
{
    public List<ClassDeclarationSyntax> MapperClasses { get; } = new();
    public List<ClassDeclarationSyntax> ResultErrorClasses { get; } = new();
    public List<string> RequiredNamespaces { get; } = new();

    
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        // Look for class declarations that implement the specified interface
        if (syntaxNode is ClassDeclarationSyntax classDeclaration)
        {
            foreach (var baseType in classDeclaration.BaseList?.Types ?? new SeparatedSyntaxList<BaseTypeSyntax>())
            {
                if (baseType.Type is GenericNameSyntax nameSyntax && nameSyntax.Identifier.Text.StartsWith("IResultErrorMapper"))
                {
                    MapperClasses.Add(classDeclaration);
                    RequiredNamespaces.Add(GetNamespace(classDeclaration));
                    break;
                }
                
                if (baseType.Type is IdentifierNameSyntax identifierNameSyntax && identifierNameSyntax.Identifier.Text == "IResultError")
                {
                    ResultErrorClasses.Add(classDeclaration);
                    RequiredNamespaces.Add(GetNamespace(classDeclaration));
                    break;
                }
            }
        }
    }
    
    private static string GetNamespace(ClassDeclarationSyntax classDeclaration)
    {
        var namespaceDeclaration = classDeclaration.Parent;
        
        while (namespaceDeclaration != null && !(namespaceDeclaration is NamespaceDeclarationSyntax) && !(namespaceDeclaration is FileScopedNamespaceDeclarationSyntax))
        {
            namespaceDeclaration = namespaceDeclaration.Parent;
        }
        
        return namespaceDeclaration switch
        {
            NamespaceDeclarationSyntax namespaceSyntax => namespaceSyntax.Name.ToString(),
            FileScopedNamespaceDeclarationSyntax fileScopedNamespaceSyntax => fileScopedNamespaceSyntax.Name.ToString(),
            _ => string.Empty
        };
    }
}
using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Simplification;

namespace CSharpFunctionalExtensions.HttpResults.Generators;

/// <summary>
///   Migrates a result error mapper to request-service resolution by replacing its directly implemented
///   IResultErrorMapper interface with IServiceResultErrorMapper and adding the required imports.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ServiceMapperCodeFixProvider)), Shared]
public sealed class ServiceMapperCodeFixProvider : CodeFixProvider
{
  private const string ResultMapperMetadataName = "CSharpFunctionalExtensions.HttpResults.IResultErrorMapper`2";
  private const string ServiceResultMapperMetadataName =
    "CSharpFunctionalExtensions.HttpResults.IServiceResultErrorMapper`2";

  public override ImmutableArray<string> FixableDiagnosticIds => ["CFEHTTPR004"];

  public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

  public override async Task RegisterCodeFixesAsync(CodeFixContext context)
  {
    var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
    var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
    var mapperDefinition = semanticModel?.Compilation.GetTypeByMetadataName(ResultMapperMetadataName);
    var serviceMapperDefinition = semanticModel?.Compilation.GetTypeByMetadataName(ServiceResultMapperMetadataName);
    var classDeclaration = root?.FindNode(context.Span).FirstAncestorOrSelf<ClassDeclarationSyntax>();

    if (
      root is null
      || semanticModel is null
      || mapperDefinition is null
      || serviceMapperDefinition is null
      || classDeclaration?.BaseList is null
    )
      return;

    var mapperBase = classDeclaration.BaseList.Types.FirstOrDefault(baseType =>
      semanticModel.GetTypeInfo(baseType.Type, context.CancellationToken).Type is INamedTypeSymbol type
      && SymbolEqualityComparer.Default.Equals(type.OriginalDefinition, mapperDefinition)
    );

    if (mapperBase is null)
      return;

    context.RegisterCodeFix(
      CodeAction.Create(
        "Use IServiceResultErrorMapper (generated mappings require httpContext on failure)",
        cancellationToken =>
          ReplaceInterfaceAsync(
            context.Document,
            root,
            semanticModel,
            serviceMapperDefinition,
            mapperBase,
            cancellationToken
          ),
        nameof(ServiceMapperCodeFixProvider)
      ),
      context.Diagnostics
    );
  }

  private static Task<Document> ReplaceInterfaceAsync(
    Document document,
    SyntaxNode root,
    SemanticModel semanticModel,
    INamedTypeSymbol serviceMapperDefinition,
    BaseTypeSyntax mapperBase,
    CancellationToken cancellationToken
  )
  {
    // RegisterCodeFixesAsync only calls this method for a semantically resolved closed mapper interface.
    var mapperType = (INamedTypeSymbol)semanticModel.GetTypeInfo(mapperBase.Type, cancellationToken).Type!;

    var serviceMapperType = serviceMapperDefinition.Construct(mapperType.TypeArguments.ToArray());
    var replacement = ((TypeSyntax)SyntaxGenerator.GetGenerator(document).TypeExpression(serviceMapperType))
      .WithAdditionalAnnotations(Simplifier.Annotation, Simplifier.AddImportsAnnotation)
      .WithTriviaFrom(mapperBase.Type);

    return Task.FromResult(document.WithSyntaxRoot(root.ReplaceNode(mapperBase.Type, replacement)));
  }
}

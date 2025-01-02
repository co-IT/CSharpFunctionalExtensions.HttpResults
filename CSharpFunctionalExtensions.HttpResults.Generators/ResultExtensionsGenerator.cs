using System.Diagnostics;
using System.Text;
using CSharpFunctionalExtensions.HttpResults.Generators.Builders;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CSharpFunctionalExtensions.HttpResults.Generators;

/// <summary>
///   A source generator that creates extension methods for mapping errors to results using classes implementing
///   <see cref="IResultErrorMapper{T}" />.
/// </summary>
[Generator]
internal class ResultExtensionsGenerator : IIncrementalGenerator
{
  private const string ResultErrorMapperInterface = "IResultErrorMapper";

  /// <summary>
  ///   Initializes the source generator.
  /// </summary>
  /// <param name="context">The initialization context for the generator.</param>
  public void Initialize(IncrementalGeneratorInitializationContext context)
  {
    var classDeclarations = context
      .SyntaxProvider.CreateSyntaxProvider(
        static (node, _) => node is ClassDeclarationSyntax,
        static (context, _) =>
        {
          var classDeclaration = (ClassDeclarationSyntax)context.Node;
          var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration) as ITypeSymbol;
          return (ClassDeclaration: classDeclaration, ClassSymbol: classSymbol);
        }
      )
      .Where(static x => x.ClassSymbol != null && ImplementsResultErrorMapper(x.ClassSymbol))
      .Select(static (x, _) => x.ClassDeclaration);

    var compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

    context.RegisterSourceOutput(
      compilationAndClasses,
      static (context, source) =>
      {
        var (compilation, classDeclarations) = source;

        var mapperClasses = new List<ClassDeclarationSyntax>();
        var requiredNamespaces = new HashSet<string>();

        Parallel.ForEach(
          classDeclarations,
          classDeclaration =>
          {
            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            var namespaceName = GetNamespace(classDeclaration, semanticModel);

            lock (mapperClasses)
            {
              mapperClasses.Add(classDeclaration);
              requiredNamespaces.Add(namespaceName);
            }
          }
        );

        if (!ResultExtensionsGeneratorValidator.CheckRules(mapperClasses, context))
          return;

        var classBuilders = new List<ClassBuilder>
        {
          new ResultExtensionsClassBuilder(requiredNamespaces, mapperClasses),
          new UnitResultExtensionsClassBuilder(requiredNamespaces, mapperClasses),
        };

        foreach (var classBuilder in classBuilders)
          context.AddSource(classBuilder.SourceFileName, SourceText.From(classBuilder.Build(), Encoding.UTF8));
      }
    );
  }

  /// <summary>
  ///   Checks if a class implements the <see cref="IResultErrorMapper{T}" /> interface.
  /// </summary>
  /// <param name="classSymbol">The symbol representing the class.</param>
  /// <returns>True if the class implements the interface; otherwise, false.</returns>
  private static bool ImplementsResultErrorMapper(ITypeSymbol? classSymbol)
  {
    if (classSymbol is null)
      return false;

    // Check all interfaces (direct and indirect)
    return classSymbol.AllInterfaces.Any(interfaceSymbol =>
      interfaceSymbol.Name.StartsWith(ResultErrorMapperInterface)
    );
  }

  /// <summary>
  ///   Retrieves the namespace of a class declaration.
  /// </summary>
  /// <param name="classDeclaration">The class declaration syntax node.</param>
  /// <param name="semanticModel">The semantic model for the syntax tree.</param>
  /// <returns>The namespace of the class, or an empty string if the namespace cannot be determined.</returns>
  private static string GetNamespace(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
  {
    var symbol = semanticModel.GetDeclaredSymbol(classDeclaration);
    return symbol?.ContainingNamespace?.ToString() ?? string.Empty;
  }
}

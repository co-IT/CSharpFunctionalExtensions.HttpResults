using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests;

public static class GeneratorTestHelper
{
  public static (IEnumerable<Diagnostic> Diagnostics, string GeneratedSource) RunGenerator(
    string sourceCode,
    IEnumerable<MetadataReference>? additionalReferences = null
  )
  {
    var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

    var references = new List<MetadataReference>
    {
      MetadataReference.CreateFromFile(typeof(ResultExtensionsGenerator).Assembly.Location),
      MetadataReference.CreateFromFile(typeof(IResultErrorMapper<,>).Assembly.Location),
    };

    if (additionalReferences != null)
      references.AddRange(additionalReferences);

    var compilation = CSharpCompilation.Create(
      "TestAssembly",
      [syntaxTree],
      references.OfType<PortableExecutableReference>(),
      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
    );

    var generator = new ResultExtensionsGenerator();
    var driver = CSharpGeneratorDriver.Create(generator);

    driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

    var sourceFiles = outputCompilation
      .SyntaxTrees.Where(tree => tree.FilePath.EndsWith(".g.cs", StringComparison.OrdinalIgnoreCase))
      .Select(tree => tree.GetText().ToString());

    var generatedSource = string.Join("\n\n", sourceFiles);

    return (diagnostics, generatedSource);
  }
}

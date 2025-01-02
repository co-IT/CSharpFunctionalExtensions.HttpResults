using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests;

public static class ResultExtensionsGeneratorTestHelper
{
  public static IEnumerable<Diagnostic> RunGenerator(string sourceCode)
  {
    var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

    var references = new[]
    {
      MetadataReference.CreateFromFile(typeof(ResultExtensionsGenerator).Assembly.Location),
      MetadataReference.CreateFromFile(typeof(IResultErrorMapper<,>).Assembly.Location),
    }.ToList();

    var compilation = CSharpCompilation.Create(
      "TestAssembly",
      [syntaxTree],
      references,
      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
    );

    var generator = new ResultExtensionsGenerator();
    var driver = CSharpGeneratorDriver.Create(generator);

    driver.RunGeneratorsAndUpdateCompilation(compilation, out _, out var diagnostics);

    return diagnostics;
  }
}

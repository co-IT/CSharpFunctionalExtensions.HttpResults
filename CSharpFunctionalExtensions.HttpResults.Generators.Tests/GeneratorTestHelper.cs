using System.Collections.Immutable;
using AwesomeAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests;

public static class GeneratorTestHelper
{
  private static readonly CSharpParseOptions ParseOptions = CSharpParseOptions
    .Default.WithLanguageVersion(LanguageVersion.Preview)
    .WithPreprocessorSymbols("NET10_0_OR_GREATER");

  public static (IEnumerable<Diagnostic> Diagnostics, string GeneratedSource) RunGenerator(
    string sourceCode,
    IEnumerable<MetadataReference>? additionalReferences = null
  )
  {
    var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode, ParseOptions);

    var references = new List<MetadataReference>();

    // The trusted platform assemblies include the ASP.NET Core shared framework parts
    // required to resolve types like ProblemHttpResult during generation.
    var trustedPlatformAssemblies = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(
      Path.PathSeparator
    );
    references.AddRange(trustedPlatformAssemblies.Select(path => MetadataReference.CreateFromFile(path)));

    if (additionalReferences != null)
      references.AddRange(additionalReferences);

    var compilation = CSharpCompilation.Create(
      "TestAssembly",
      [syntaxTree],
      DistinctReferences(references),
      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
    );

    var generator = new ResultExtensionsGenerator();
    var driver = CSharpGeneratorDriver.Create(
      [generator.AsSourceGenerator()],
      parseOptions: (CSharpParseOptions)syntaxTree.Options
    );

    driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generatorDiagnostics);

    var sourceFiles = outputCompilation
      .SyntaxTrees.Where(tree => tree.FilePath.EndsWith(".g.cs", StringComparison.OrdinalIgnoreCase))
      .Select(tree => tree.GetText().ToString());

    var generatedSource = string.Join("\n\n", sourceFiles);

    return (CollectDiagnostics(generatorDiagnostics, outputCompilation), generatedSource);
  }

  /// <summary>
  ///   Runs the generator and returns the generated sources keyed by their hint name,
  ///   e.g. <c>ResultExtensions.g.cs</c>. Assertions can target individual generated files.
  /// </summary>
  public static (IEnumerable<Diagnostic> Diagnostics, IReadOnlyDictionary<string, string> Files) RunGeneratorPerFile(
    string sourceCode,
    IEnumerable<MetadataReference>? additionalReferences = null
  )
  {
    var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode, ParseOptions);

    var references = new List<MetadataReference>();

    var trustedPlatformAssemblies = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(
      Path.PathSeparator
    );
    references.AddRange(trustedPlatformAssemblies.Select(path => MetadataReference.CreateFromFile(path)));

    if (additionalReferences != null)
      references.AddRange(additionalReferences);

    var compilation = CSharpCompilation.Create(
      "TestAssembly",
      [syntaxTree],
      DistinctReferences(references),
      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
    );

    var driver = CSharpGeneratorDriver.Create(
      [new ResultExtensionsGenerator().AsSourceGenerator()],
      parseOptions: (CSharpParseOptions)syntaxTree.Options
    );
    driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generatorDiagnostics);

    var files = outputCompilation
      .SyntaxTrees.Where(tree => tree.FilePath.EndsWith(".g.cs", StringComparison.OrdinalIgnoreCase))
      .ToDictionary(tree => Path.GetFileName(tree.FilePath), tree => tree.GetText().ToString());

    return (CollectDiagnostics(generatorDiagnostics, outputCompilation), files);
  }

  /// <summary>Compiles the given source into an assembly reference, e.g. to simulate referenced mapper libraries.</summary>
  public static MetadataReference CreateReference(string assemblyName, string source)
  {
    var trustedPlatformAssemblies = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
      .Split(Path.PathSeparator)
      .Select(path => MetadataReference.CreateFromFile(path));
    var compilation = CSharpCompilation.Create(
      assemblyName,
      [CSharpSyntaxTree.ParseText(source)],
      trustedPlatformAssemblies.Append(
        MetadataReference.CreateFromFile(typeof(IResultErrorMapper<,>).Assembly.Location)
      ),
      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
    );
    using var stream = new MemoryStream();
    compilation.Emit(stream).Success.Should().BeTrue();
    return MetadataReference.CreateFromImage(stream.ToArray());
  }

  private static IReadOnlyList<Diagnostic> CollectDiagnostics(
    ImmutableArray<Diagnostic> generatorDiagnostics,
    Compilation outputCompilation
  ) =>
    generatorDiagnostics
      .Concat(outputCompilation.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error))
      .GroupBy(diagnostic => (diagnostic.Id, diagnostic.Location.SourceSpan, diagnostic.GetMessage()))
      .Select(group => group.First())
      .ToArray();

  private static IEnumerable<PortableExecutableReference> DistinctReferences(IEnumerable<MetadataReference> references)
  {
    var portableReferences = references.OfType<PortableExecutableReference>().ToArray();
    var fileReferences = portableReferences
      .Where(reference => reference.FilePath is not null)
      .GroupBy(reference => reference.FilePath!, StringComparer.OrdinalIgnoreCase)
      .Select(group => group.First());

    // References created from in-memory images have no path and must not be collapsed into one group.
    return fileReferences.Concat(portableReferences.Where(reference => reference.FilePath is null));
  }
}

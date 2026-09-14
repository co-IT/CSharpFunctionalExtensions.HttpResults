using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests;

public class ServiceMapperCodeFixProviderTests
{
  [Fact]
  public void Metadata_exposes_the_supported_diagnostic_and_batch_fixer()
  {
    var provider = new ServiceMapperCodeFixProvider();

    provider.FixableDiagnosticIds.Should().Equal("CFEHTTPR004");
    provider.GetFixAllProvider().Should().BeSameAs(WellKnownFixAllProviders.BatchFixer);
  }

  [Fact]
  public async Task Fix_replaces_only_the_result_mapper_interface_and_preserves_mapper_source()
  {
    const string source = """
      using CSharpFunctionalExtensions.HttpResults;
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;

      public sealed class MyError;

      // This mapper needs constructor injection.
      public sealed class MyMapper /* keep this comment */ : IResultErrorMapper<MyError, ProblemHttpResult>
      {
          public MyMapper(string dependency) { }

          // Map implementation must be untouched.
          public ProblemHttpResult Map(MyError error) => TypedResults.Problem();
      }
      """;

    var fixedSource = await ApplyFixAsync(source);

    fixedSource.Should().Contain("IServiceResultErrorMapper<MyError, ProblemHttpResult>");
    fixedSource.Should().NotContain("global::");
    fixedSource.Should().Contain("/* keep this comment */");
    fixedSource.Should().Contain("public ProblemHttpResult Map(MyError error) => TypedResults.Problem();");
  }

  [Fact]
  public async Task No_fix_is_offered_when_the_rule_reports_no_diagnostic()
  {
    const string source = """
      public sealed class MyMapper : SomeOtherInterface<MyError, MyHttpResult>
      {
        public SomeOtherInterface<MyError, MyHttpResult> Map(MyError error) => throw null!;
      }
      """;

    var actions = await GetActionsAsync(source);

    actions.Should().BeEmpty();
  }

  [Theory]
  [InlineData("public sealed class Mapper { }")]
  [InlineData("public sealed class Mapper : System.IDisposable { public void Dispose() { } }")]
  public async Task No_fix_is_offered_when_a_diagnostic_does_not_point_to_a_mapper_interface(string source)
  {
    var actions = await GetActionsAsync(source, createSyntheticDiagnostic: true);

    actions.Should().BeEmpty();
  }

  [Fact]
  public async Task Fixed_mapper_is_accepted_as_a_service_mapper_by_the_generator()
  {
    const string source = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      public sealed class Error;
      public sealed class Mapper : IResultErrorMapper<Error, ProblemHttpResult>
      {
          public Mapper(string dependency) { }
          public ProblemHttpResult Map(Error error) => TypedResults.Problem();
      }
      """;

    var fixedSource = await ApplyFixAsync(source);
    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(fixedSource);

    diagnostics.Should().NotContain(diagnostic => diagnostic.Id == "CFEHTTPR004");
    files.Values.Should().Contain(source => source.Contains("GetRequiredService<global::Mapper>()"));
    files["ResultErrorMapperCache.g.cs"].Should().NotContain("new global::Mapper");
  }

  [Fact]
  public async Task Fix_supports_a_qualified_interface_name()
  {
    const string source = """
      using Microsoft.AspNetCore.Http.HttpResults;

      public sealed class Error;
      public sealed class Mapper : CSharpFunctionalExtensions.HttpResults.IResultErrorMapper<Error, ProblemHttpResult>
      {
        public Mapper(string dependency) { }
        public ProblemHttpResult Map(Error error) => TypedResults.Problem();
      }
      """;

    var fixedSource = await ApplyFixAsync(source);

    fixedSource
      .Should()
      .Contain("using CSharpFunctionalExtensions.HttpResults;")
      .And.Contain("IServiceResultErrorMapper<Error, ProblemHttpResult>");
    fixedSource.Should().NotContain("global::");
  }

  [Fact]
  public async Task Fix_supports_an_alias_qualified_closed_interface()
  {
    const string source = """
      using CSharpFunctionalExtensions.HttpResults;
      using Microsoft.AspNetCore.Http.HttpResults;
      using ResultMapper = CSharpFunctionalExtensions.HttpResults.IResultErrorMapper<Error, Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult>;

      public sealed class Error;
      public sealed class Mapper : ResultMapper
      {
        public Mapper(string dependency) { }
        public ProblemHttpResult Map(Error error) => TypedResults.Problem();
      }
      """;

    var fixedSource = await ApplyFixAsync(source);

    fixedSource.Should().NotContain(": ResultMapper");
    fixedSource.Should().Contain("IServiceResultErrorMapper<Error, ProblemHttpResult>");
    fixedSource.Should().NotContain("global::");
  }

  private static async Task<string> ApplyFixAsync(string source)
  {
    var actions = await GetActionsAsync(source);
    actions.Should().ContainSingle();
    actions[0].Title.Should().Be("Use IServiceResultErrorMapper (generated mappings require httpContext on failure)");

    var operations = await actions[0].GetOperationsAsync(TestContext.Current.CancellationToken);
    var changedSolution = operations.OfType<ApplyChangesOperation>().Should().ContainSingle().Which.ChangedSolution;
    var changedDocument = changedSolution.Projects.Single().Documents.Single();
    return (await changedDocument.GetTextAsync(TestContext.Current.CancellationToken)).ToString();
  }

  /// <summary>
  ///   Produces the code-fix actions exactly as the IDE would: diagnostics come from the real generator,
  ///   so their spans match production.
  /// </summary>
  private static async Task<List<CodeAction>> GetActionsAsync(string source, bool createSyntheticDiagnostic = false)
  {
    using var workspace = new AdhocWorkspace();
    var project = workspace
      .AddProject("TestProject", LanguageNames.CSharp)
      .WithParseOptions(new CSharpParseOptions(LanguageVersion.Preview))
      .WithMetadataReferences(CreateReferences());
    workspace.TryApplyChanges(project.Solution).Should().BeTrue();
    project = workspace.CurrentSolution.GetProject(project.Id)!;
    var document = workspace.AddDocument(project.Id, "Mapper.cs", SourceText.From(source));
    var compilation = (await document.Project.GetCompilationAsync(TestContext.Current.CancellationToken))!;
    var parseOptions = (CSharpParseOptions)compilation.SyntaxTrees.First().Options;
    var driver = CSharpGeneratorDriver.Create(
      [new ResultExtensionsGenerator().AsSourceGenerator()],
      parseOptions: parseOptions
    );
    driver.RunGeneratorsAndUpdateCompilation(compilation, out _, out var generatorDiagnostics);
    var diagnostics = createSyntheticDiagnostic
      ? [CreateSyntheticDiagnostic(compilation.SyntaxTrees.Single())]
      : generatorDiagnostics.Where(diagnostic => diagnostic.Id == "CFEHTTPR004").ToArray();

    var actions = new List<CodeAction>();
    foreach (var diagnostic in diagnostics)
    {
      var context = new CodeFixContext(
        document,
        diagnostic,
        (action, _) => actions.Add(action),
        TestContext.Current.CancellationToken
      );

      await new ServiceMapperCodeFixProvider().RegisterCodeFixesAsync(context);
    }

    return actions;
  }

  private static Diagnostic CreateSyntheticDiagnostic(SyntaxTree syntaxTree)
  {
    var classDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Single();
    var descriptor = new DiagnosticDescriptor(
      "CFEHTTPR004",
      "Test diagnostic",
      "Test diagnostic",
      "Test",
      DiagnosticSeverity.Error,
      isEnabledByDefault: true
    );

    return Diagnostic.Create(descriptor, classDeclaration.Identifier.GetLocation());
  }

  private static IReadOnlyList<MetadataReference> CreateReferences()
  {
    var references = new List<MetadataReference>
    {
      MetadataReference.CreateFromFile(typeof(IResultErrorMapper<,>).Assembly.Location),
    };

    var trustedPlatformAssemblies = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(
      Path.PathSeparator
    );
    references.AddRange(trustedPlatformAssemblies.Select(path => MetadataReference.CreateFromFile(path)));

    return references;
  }
}

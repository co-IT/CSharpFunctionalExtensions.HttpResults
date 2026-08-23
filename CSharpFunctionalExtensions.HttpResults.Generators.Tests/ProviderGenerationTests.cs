using AwesomeAssertions;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests;

public class ProviderGenerationTests
{
  private const string ProviderUsings = """
    using CSharpFunctionalExtensions.HttpResults;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    """;

  [Fact]
  public void Multiple_providers_report_an_error_and_disable_provider_generation()
  {
    var source =
      ProviderUsings
      + """

        public sealed class FirstProvider : IResultProblemDetailsProvider
        {
          public ProblemDetails CreateProblemDetails(HttpContext context, string error, int statusCode) => new();
        }
        public sealed class SecondProvider : IResultProblemDetailsProvider
        {
          public ProblemDetails CreateProblemDetails(HttpContext context, string error, int statusCode) => new();
        }
        """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().ContainSingle(diagnostic => diagnostic.Id == "CFEHTTPR005");
    files["StringErrorResultExtensions.g.cs"].Should().NotContain("HttpContext httpContext");
    files["ServiceCollectionExtensions.g.cs"].Should().NotContain("FirstProvider").And.NotContain("SecondProvider");
  }

  [Fact]
  public void Abstract_provider_is_reported_without_suppressing_static_mappings()
  {
    var source =
      ProviderUsings
      + """

        public abstract class AbstractProvider : IResultProblemDetailsProvider
        {
          public ProblemDetails CreateProblemDetails(HttpContext context, string error, int statusCode) => new();
        }
        """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().ContainSingle(diagnostic => diagnostic.Id == "CFEHTTPR007");
    files["StringErrorResultExtensions.g.cs"].Should().Contain("ProblemDetailsMappingProvider.FindMapping");
  }

  [Fact]
  public void Partial_provider_is_discovered_once()
  {
    var source =
      ProviderUsings
      + """

        public sealed partial class Provider : IResultProblemDetailsProvider;
        public sealed partial class Provider
        {
          public ProblemDetails CreateProblemDetails(HttpContext context, string error, int statusCode) => new();
        }
        """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().BeEmpty();
    files["ServiceCollectionExtensions.g.cs"].Split("global::Provider").Should().HaveCount(2);
  }

  [Fact]
  public void Referenced_provider_is_fully_qualified_and_auto_registered()
  {
    var external =
      ProviderUsings
      + """

        namespace External;
        public sealed class Provider : IResultProblemDetailsProvider
        {
          public ProblemDetails CreateProblemDetails(HttpContext context, string error, int statusCode) => new();
        }
        """;
    var reference = GeneratorTestHelper.CreateReference("ExternalProvider", external);

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile("public static class Empty;", [reference]);

    diagnostics.Should().BeEmpty();
    files["StringErrorResultExtensions.g.cs"].Should().Contain("HttpContext httpContext");
    files["ServiceCollectionExtensions.g.cs"]
      .Should()
      .Contain(
        "TryAddScoped<global::CSharpFunctionalExtensions.HttpResults.IResultProblemDetailsProvider, global::External.Provider>()"
      );
  }

  [Fact]
  public void Providers_from_multiple_referenced_assemblies_report_an_error()
  {
    var firstReference = GeneratorTestHelper.CreateReference(
      "FirstProviderAssembly",
      ProviderUsings
        + """

        namespace First;
        public sealed class Provider : IResultProblemDetailsProvider
        {
          public ProblemDetails CreateProblemDetails(HttpContext context, string error, int statusCode) => new();
        }
        """
    );
    var secondReference = GeneratorTestHelper.CreateReference(
      "SecondProviderAssembly",
      ProviderUsings
        + """

        namespace Second;
        public sealed class Provider : IResultProblemDetailsProvider
        {
          public ProblemDetails CreateProblemDetails(HttpContext context, string error, int statusCode) => new();
        }
        """
    );

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(
      "public static class Empty;",
      [firstReference, secondReference]
    );

    diagnostics.Should().ContainSingle(diagnostic => diagnostic.Id == "CFEHTTPR005");
    files["StringErrorResultExtensions.g.cs"].Should().NotContain("HttpContext httpContext");
  }
}

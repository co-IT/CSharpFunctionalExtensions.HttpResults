using AwesomeAssertions;
using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests;

public class StringErrorMethodGenerationTests
{
  private const string NoProviderSource = "public static class Empty { }";

  internal const string WithProviderSource = """
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using CSharpFunctionalExtensions.HttpResults;

    public sealed class MyProvider : IResultProblemDetailsProvider
    {
      public ProblemDetails CreateProblemDetails(HttpContext httpContext, string error, int statusCode) =>
        new ProblemDetails { Status = statusCode, Detail = error };
    }
    """;

  [Fact]
  public void Static_variants_are_always_emitted_into_the_well_known_namespace()
  {
    var (_, files) = GeneratorTestHelper.RunGeneratorPerFile(NoProviderSource);
    var builtin = files["StringErrorResultExtensions.g.cs"];

    builtin.Should().Contain("namespace CSharpFunctionalExtensions.HttpResults.ResultExtensions;");
    builtin.Should().Contain("public static partial class ResultExtensions {");

    builtin
      .Should()
      .Contain(
        "ToStatusCodeHttpResult(this Result result, int successStatusCode = 204, int failureStatusCode = 400, Action<ProblemDetails>? customizeProblemDetails = null)"
      );
    builtin
      .Should()
      .Contain(
        "ToOkHttpResult<T>(this Result<T> result, int failureStatusCode = 400, Action<ProblemDetails>? customizeProblemDetails = null)"
      );
    builtin
      .Should()
      .Contain(
        "ToAcceptedHttpResult<T>(this Result<T> result, Func<T, Uri> uri, int failureStatusCode = 400, Action<ProblemDetails>? customizeProblemDetails = null)"
      );
    builtin
      .Should()
      .Contain(
        "ToContentHttpResult(this Result<string> result, string? contentType = null, Encoding? contentEncoding = null, int? statusCode = null, int failureStatusCode = 400, Action<ProblemDetails>? customizeProblemDetails = null)"
      );

    builtin.Should().Contain("ProblemDetailsMappingProvider.FindMapping(failureStatusCode)");
  }

  [Fact]
  public void Context_overloads_are_emitted_when_a_provider_exists_in_the_compilation()
  {
    var (_, files) = GeneratorTestHelper.RunGeneratorPerFile(WithProviderSource);
    var builtin = files["StringErrorResultExtensions.g.cs"];

    builtin.Should().Contain("using Microsoft.Extensions.DependencyInjection;");
    builtin
      .Should()
      .Contain(
        "ToOkHttpResult<T>(this Result<T> result, HttpContext httpContext, int failureStatusCode = 400, Action<ProblemDetails>? customizeProblemDetails = null)"
      );
    builtin
      .Should()
      .Contain(
        "ToAcceptedHttpResult<T>(this Result<T> result, Func<T, Uri> uri, HttpContext httpContext, int failureStatusCode = 400, Action<ProblemDetails>? customizeProblemDetails = null)"
      );
    builtin
      .Should()
      .Contain(
        "ToStatusCodeHttpResult(this Result result, HttpContext httpContext, int successStatusCode = 204, int failureStatusCode = 400, Action<ProblemDetails>? customizeProblemDetails = null)"
      );

    builtin.Should().Contain("httpContext.RequestServices.GetRequiredService<IResultProblemDetailsProvider>()");
    builtin.Should().Contain("CreateProblemDetails(httpContext, result.Error, failureStatusCode)");

    builtin.Split("customizeProblemDetails?.Invoke(problemDetails);").Should().HaveCount(29);

    builtin
      .Should()
      .Contain("ToOkHttpResult<T>(await result, httpContext, failureStatusCode, customizeProblemDetails)");
  }

  [Fact]
  public void Context_overloads_are_not_emitted_without_a_provider()
  {
    var (_, files) = GeneratorTestHelper.RunGeneratorPerFile(NoProviderSource);
    var builtin = files["StringErrorResultExtensions.g.cs"];

    builtin.Should().NotContain("HttpContext httpContext");
    builtin.Should().NotContain("GetRequiredService");
    builtin.Should().NotContain("Microsoft.Extensions.DependencyInjection");
  }

  [Fact]
  public void Public_provider_from_referenced_assembly_enables_emission_internal_does_not()
  {
    const string externalPublic = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Mvc;
      using CSharpFunctionalExtensions.HttpResults;

      namespace External;

      public sealed class PublicProvider : IResultProblemDetailsProvider
      {
        public ProblemDetails CreateProblemDetails(HttpContext httpContext, string error, int statusCode) => new();
      }
      """;

    string externalInternal = externalPublic.Replace("public sealed class", "internal sealed class");

    var publicReference = GeneratorTestHelper.CreateReference("ExternalProvidersPublic", externalPublic);
    var (_, filesWithPublic) = GeneratorTestHelper.RunGeneratorPerFile(NoProviderSource, [publicReference]);
    filesWithPublic["StringErrorResultExtensions.g.cs"].Should().Contain("HttpContext httpContext");

    var internalReference = GeneratorTestHelper.CreateReference("ExternalProvidersInternal", externalInternal);
    var (_, filesWithInternal) = GeneratorTestHelper.RunGeneratorPerFile(NoProviderSource, [internalReference]);
    filesWithInternal["StringErrorResultExtensions.g.cs"].Should().NotContain("HttpContext httpContext");
  }

  [Fact]
  public void ServerSentEvents_family_is_wrapped_in_net10_conditional()
  {
    var (_, files) = GeneratorTestHelper.RunGeneratorPerFile(NoProviderSource);
    var builtin = files["StringErrorResultExtensions.g.cs"];

    builtin.Should().Contain("#if NET10_0_OR_GREATER");
    builtin.Should().Contain("ServerSentEventsResult<T>");
  }
}

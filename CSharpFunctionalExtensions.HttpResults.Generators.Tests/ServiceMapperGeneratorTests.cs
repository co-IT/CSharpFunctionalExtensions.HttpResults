using AwesomeAssertions;
using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests;

public class ServiceMapperGeneratorTests
{
  private const string ServiceMapperSource = """
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.HttpResults;
    using CSharpFunctionalExtensions.HttpResults;

    namespace Sample;

    public sealed class SampleError;

    public sealed class Dependency;

    public sealed class ServiceMapper(Dependency dependency) : IServiceResultErrorMapper<SampleError, ProblemHttpResult>
    {
      public ProblemHttpResult Map(SampleError error) => TypedResults.Problem(detail: dependency.ToString());
    }
    """;

  [Fact]
  public void ServiceMapper_generates_required_context_overloads_for_every_mapping_family()
  {
    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(ServiceMapperSource);

    diagnostics.Should().BeEmpty();
    var mapperMethods = files["ResultExtensions.g.cs"] + files["UnitResultExtensions.g.cs"];

    mapperMethods.Should().NotContain("ResultErrorMapperCache.Mapper");
    mapperMethods.Should().Contain("GetRequiredService<global::Sample.ServiceMapper>()");
    mapperMethods
      .Should()
      .Contain(
        "global::CSharpFunctionalExtensions.HttpResults.IResultErrorMapper<global::Sample.SampleError, global::Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult>"
      );
    mapperMethods.Should().Contain("A HttpContext is required for IServiceResultErrorMapper mappings.");

    // 12 Result<T,E> families and 2 UnitResult<E> families, each sync and async:
    // 28 overloads carrying a REQUIRED HttpContext parameter (no default value).
    mapperMethods.Split("HttpContext httpContext").Should().HaveCount(29);
    mapperMethods.Should().NotContain("HttpContext? httpContext");
    mapperMethods.Should().NotContain("httpContext = null");

    // Every async overload forwards to its sync counterpart.
    mapperMethods.Split("await result").Should().HaveCount(15);

    // The mapper returns ProblemHttpResult: every sync failure branch applies the
    // callback to the mapped result (14 families).
    (files["ResultExtensions.g.cs"].Split("customizeProblemDetails?.Invoke(mapped.ProblemDetails);").Length - 1)
      .Should()
      .Be(12);
    (files["UnitResultExtensions.g.cs"].Split("customizeProblemDetails?.Invoke(mapped.ProblemDetails);").Length - 1)
      .Should()
      .Be(2);
  }

  [Fact]
  public void ServiceMapper_overloads_place_httpContext_between_required_and_optional_parameters()
  {
    var (_, files) = GeneratorTestHelper.RunGeneratorPerFile(ServiceMapperSource);
    var mapperMethods = files["ResultExtensions.g.cs"] + files["UnitResultExtensions.g.cs"];

    // Accepted requires uri -> context comes after it.
    mapperMethods
      .Should()
      .Contain(
        "ToAcceptedHttpResult<T>(this Result<T,global::Sample.SampleError> result, Func<T, Uri> uri, HttpContext httpContext, Action<ProblemDetails>? customizeProblemDetails = null)"
      );

    // StatusCode only has optional parameters -> context comes first.
    mapperMethods
      .Should()
      .Contain(
        "ToStatusCodeHttpResult<T>(this Result<T,global::Sample.SampleError> result, HttpContext httpContext, int successStatusCode = 204, Action<ProblemDetails>? customizeProblemDetails = null)"
      );

    mapperMethods
      .Should()
      .Contain(
        "ToNoContentHttpResult(this UnitResult<global::Sample.SampleError> result, HttpContext httpContext, Action<ProblemDetails>? customizeProblemDetails = null)"
      );

    // File/FileStream forward context between the family parameters and the callback,
    // mirroring the sync parameter order.
    mapperMethods
      .Should()
      .Contain(
        "ToFileHttpResult(await result, httpContext, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing, customizeProblemDetails)"
      );
    mapperMethods
      .Should()
      .Contain(
        "ToFileStreamHttpResult<T>(await result, httpContext, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing, customizeProblemDetails)"
      );
  }

  [Fact]
  public void ServiceMapper_with_constructor_dependency_does_not_report_standard_constructor_diagnostic()
  {
    var (diagnostics, _) = GeneratorTestHelper.RunGeneratorPerFile(ServiceMapperSource);

    diagnostics.Should().NotContain(diagnostic => diagnostic.Id == "CFEHTTPR004");
  }

  [Fact]
  public void ServiceMapper_without_ProblemHttpResult_return_type_gets_no_customize_parameter()
  {
    const string source = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      namespace Sample;

      public sealed class OtherError;

      public sealed class Service;

      public sealed class OtherMapper(Service service) : IServiceResultErrorMapper<OtherError, NotFound<string>>
      {
        public NotFound<string> Map(OtherError error) => TypedResults.NotFound(error.ToString());
      }
      """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);
    var mapperMethods = files["ResultExtensions.g.cs"];

    diagnostics.Should().BeEmpty();
    mapperMethods.Should().NotContain("customizeProblemDetails");
    mapperMethods.Should().Contain("(this Result<T,global::Sample.OtherError> result, HttpContext httpContext)");
    mapperMethods
      .Should()
      .Contain("httpContext.RequestServices.GetRequiredService<global::Sample.OtherMapper>()).Map(result.Error);");
  }

  [Fact]
  public void StandardMapper_retains_static_instance_and_context_free_overloads()
  {
    const string source = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      public sealed class StandardDomainError;
      public sealed class StandardMapper : IResultErrorMapper<StandardDomainError, ProblemHttpResult>
      {
        public ProblemHttpResult Map(StandardDomainError error) => TypedResults.Problem();
      }
      """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().BeEmpty();
    files["ResultErrorMapperCache.g.cs"]
      .Should()
      .Contain(
        "internal static global::CSharpFunctionalExtensions.HttpResults.IResultErrorMapper<global::StandardDomainError, global::Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult> Mapper0 { get; } = new global::StandardMapper();"
      );

    var mapperMethods = files["ResultExtensions.g.cs"];
    mapperMethods.Should().NotContain("GetRequiredService<");
    mapperMethods.Should().NotContain("HttpContext");
    mapperMethods
      .Should()
      .Contain("var mapped = CSharpFunctionalExtensionsHttpResultsResultErrorMapperCache.Mapper0.Map(result.Error);");

    // Standard mappers returning ProblemHttpResult get the optional customize parameter.
    mapperMethods.Should().Contain("Action<ProblemDetails>? customizeProblemDetails = null");
    mapperMethods.Should().Contain("customizeProblemDetails?.Invoke(mapped.ProblemDetails);");
  }

  [Fact]
  public void StandardMapper_without_ProblemHttpResult_return_type_stays_parameterless()
  {
    const string source = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      public sealed class StandardDomainError;
      public sealed class StandardNotFoundMapper : IResultErrorMapper<StandardDomainError, NotFound<string>>
      {
        public NotFound<string> Map(StandardDomainError error) => TypedResults.NotFound(string.Empty);
      }
      """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().BeEmpty();
    files["UnitResultExtensions.g.cs"].Should().Contain("(this UnitResult<global::StandardDomainError> result)");
    files["UnitResultExtensions.g.cs"]
      .Should()
      .Contain("return CSharpFunctionalExtensionsHttpResultsResultErrorMapperCache.Mapper0.Map(result.Error);");
  }

  [Fact]
  public void Duplicate_error_type_is_reported_for_mixed_standard_and_service_mappers()
  {
    const string source = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      public sealed class SharedError;
      public sealed class StandardMapper : IResultErrorMapper<SharedError, ProblemHttpResult>
      {
        public ProblemHttpResult Map(SharedError error) => TypedResults.Problem();
      }
      public sealed class ServiceMapper : IServiceResultErrorMapper<SharedError, ProblemHttpResult>
      {
        public ProblemHttpResult Map(SharedError error) => TypedResults.Problem();
      }
      """;

    var (diagnostics, _) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().ContainSingle(diagnostic => diagnostic.Id == "CFEHTTPR002");
  }

  [Fact]
  public void Error_types_with_the_same_short_name_in_different_namespaces_are_not_duplicates()
  {
    const string source = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      namespace First { public sealed class Error; }
      namespace Second { public sealed class Error; }
      namespace Mappers
      {
        public sealed class FirstMapper : IResultErrorMapper<First.Error, ProblemHttpResult>
        {
          public ProblemHttpResult Map(First.Error error) => TypedResults.Problem();
        }
        public sealed class SecondMapper : IServiceResultErrorMapper<Second.Error, ProblemHttpResult>
        {
          public ProblemHttpResult Map(Second.Error error) => TypedResults.Problem();
        }
      }
      """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().NotContain(diagnostic => diagnostic.Id == "CFEHTTPR002");
    files.Should().ContainKey("ResultExtensions.g.cs");
  }
}

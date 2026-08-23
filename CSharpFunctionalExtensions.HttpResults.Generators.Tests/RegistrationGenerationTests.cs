using AwesomeAssertions;
using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests;

public class RegistrationGenerationTests
{
  [Fact]
  public void Registration_is_emitted_even_without_any_mappers()
  {
    var (_, files) = GeneratorTestHelper.RunGeneratorPerFile("public static class Empty { }");
    var registration = files["ServiceCollectionExtensions.g.cs"];

    registration
      .Should()
      .Contain(
        "public static IServiceCollection AddCSharpFunctionalExtensionsHttpResults(this IServiceCollection services)"
      );
    registration.Should().NotContain("TryAddScoped<");
  }

  [Fact]
  public void Source_service_mapper_is_registered_but_standard_mapper_is_not()
  {
    const string source = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      namespace Sample;

      public sealed class ServiceError;
      public sealed class StandardDomainError;

      public sealed class ServiceMapper : IServiceResultErrorMapper<ServiceError, ProblemHttpResult>
      {
        public ProblemHttpResult Map(ServiceError error) => TypedResults.Problem();
      }

      public sealed class StandardMapper : IResultErrorMapper<StandardDomainError, ProblemHttpResult>
      {
        public ProblemHttpResult Map(StandardDomainError error) => TypedResults.Problem();
      }
      """;

    var (_, files) = GeneratorTestHelper.RunGeneratorPerFile(source);
    var registration = files["ServiceCollectionExtensions.g.cs"];

    registration.Should().Contain("services.TryAddScoped<global::Sample.ServiceMapper>();");
    registration.Should().NotContain("StandardMapper");
  }

  [Fact]
  public void Public_referenced_service_mapper_is_registered_internal_is_not()
  {
    const string external = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      namespace External;

      public sealed class ExternalError;

      public sealed class PublicServiceMapper : IServiceResultErrorMapper<ExternalError, ProblemHttpResult>
      {
        public ProblemHttpResult Map(ExternalError error) => TypedResults.Problem();
      }

      internal sealed class InternalServiceMapper : IServiceResultErrorMapper<ExternalError, NotFound<string>>
      {
        public NotFound<string> Map(ExternalError error) => TypedResults.NotFound(string.Empty);
      }
      """;

    var reference = GeneratorTestHelper.CreateReference("ExternalMappers", external);
    var (_, files) = GeneratorTestHelper.RunGeneratorPerFile("public static class Empty { }", [reference]);
    var registration = files["ServiceCollectionExtensions.g.cs"];

    registration.Should().Contain("services.TryAddScoped<global::External.PublicServiceMapper>();");
    registration.Should().NotContain("InternalServiceMapper");
  }

  [Fact]
  public void Discovered_provider_is_auto_registered_as_scoped()
  {
    var (_, files) = GeneratorTestHelper.RunGeneratorPerFile(StringErrorMethodGenerationTests.WithProviderSource);
    var registration = files["ServiceCollectionExtensions.g.cs"];

    registration
      .Should()
      .Contain(
        "services.TryAddScoped<global::CSharpFunctionalExtensions.HttpResults.IResultProblemDetailsProvider, global::MyProvider>();"
      );
  }

  [Fact]
  public void Registrations_are_deterministically_ordered()
  {
    const string source = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      namespace Zeta { public sealed class ZMapper : IServiceResultErrorMapper<Zeta.Error, ProblemHttpResult> { public ProblemHttpResult Map(Zeta.Error error) => TypedResults.Problem(); }
        public sealed class Error; }
      namespace Alpha { public sealed class AMapper : IServiceResultErrorMapper<Alpha.Error, ProblemHttpResult> { public ProblemHttpResult Map(Alpha.Error error) => TypedResults.Problem(); }
        public sealed class Error; }
      """;

    var (_, files) = GeneratorTestHelper.RunGeneratorPerFile(source);
    var registration = files["ServiceCollectionExtensions.g.cs"];

    var alphaIndex = registration.IndexOf("global::Alpha.AMapper", StringComparison.Ordinal);
    var zetaIndex = registration.IndexOf("global::Zeta.ZMapper", StringComparison.Ordinal);

    alphaIndex.Should().BePositive().And.BeLessThan(zetaIndex);
  }
}

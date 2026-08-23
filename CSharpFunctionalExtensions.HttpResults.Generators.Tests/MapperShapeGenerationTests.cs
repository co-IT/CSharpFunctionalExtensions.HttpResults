using AwesomeAssertions;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests;

public class MapperShapeGenerationTests
{
  [Theory]
  [InlineData(false)]
  [InlineData(true)]
  public void Explicit_interface_Map_compiles_and_is_invoked_through_the_contract(bool serviceMapper)
  {
    var marker = serviceMapper ? "IServiceResultErrorMapper" : "IResultErrorMapper";
    var source = $$"""
      using CSharpFunctionalExtensions.HttpResults;
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;

      public sealed class Error;
      public sealed class Mapper : {{marker}}<Error, ProblemHttpResult>
      {
        ProblemHttpResult IResultErrorMapper<Error, ProblemHttpResult>.Map(Error error) =>
          TypedResults.Problem();
      }
      """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().BeEmpty();
    (files["ResultExtensions.g.cs"] + files["LegacyErrorMapperCache.g.cs"])
      .Should()
      .Contain(
        "global::CSharpFunctionalExtensions.HttpResults.IResultErrorMapper<global::Error, global::Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult>"
      );
  }

  [Theory]
  [InlineData(false)]
  [InlineData(true)]
  public void Inherited_Map_compiles(bool serviceMapper)
  {
    var marker = serviceMapper ? "IServiceResultErrorMapper" : "IResultErrorMapper";
    var source = $$"""
      using CSharpFunctionalExtensions.HttpResults;
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;

      public sealed class Error;
      public abstract class MapperBase
      {
        public ProblemHttpResult Map(Error error) => TypedResults.Problem();
      }
      public sealed class Mapper : MapperBase, {{marker}}<Error, ProblemHttpResult>;
      """;

    var (diagnostics, _) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().BeEmpty();
  }

  [Fact]
  public void Partial_mapper_is_analysed_once()
  {
    const string source = """
      using CSharpFunctionalExtensions.HttpResults;
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;

      public sealed class Error;
      public sealed partial class Mapper : IResultErrorMapper<Error, ProblemHttpResult>;
      public sealed partial class Mapper
      {
        public ProblemHttpResult Map(Error error) => TypedResults.Problem();
      }
      """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().BeEmpty();
    files["LegacyErrorMapperCache.g.cs"].Split("new global::Mapper()").Should().HaveCount(2);
  }

  [Theory]
  [MemberData(nameof(UnsupportedMapperSources))]
  public void Unsupported_mapper_shapes_are_reported(string declaration)
  {
    var source = $$"""
      using CSharpFunctionalExtensions.HttpResults;
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;

      public sealed class Error;
      {{declaration}}
      """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().ContainSingle(diagnostic => diagnostic.Id == "CFEHTTPR006");
    files["StringErrorResultExtensions.g.cs"].Should().Contain("ToOkHttpResult");
    files["ResultExtensions.g.cs"].Should().NotContain("global::Error");
  }

  public static TheoryData<string> UnsupportedMapperSources =>
    new()
    {
      "public abstract class Mapper : IResultErrorMapper<Error, ProblemHttpResult> { public ProblemHttpResult Map(Error error) => TypedResults.Problem(); }",
      "public sealed class Mapper<T> : IResultErrorMapper<T, ProblemHttpResult> { public ProblemHttpResult Map(T error) => TypedResults.Problem(); }",
      "public static class Holder { private sealed class Mapper : IResultErrorMapper<Error, ProblemHttpResult> { public ProblemHttpResult Map(Error error) => TypedResults.Problem(); } }",
      "public sealed class Mapper : IServiceResultErrorMapper<Error, ProblemHttpResult> { private Mapper() { } public ProblemHttpResult Map(Error error) => TypedResults.Problem(); }",
    };

  [Fact]
  public void Legacy_mapper_with_required_members_is_rejected()
  {
    const string source = """
      using CSharpFunctionalExtensions.HttpResults;
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;

      public sealed class Error;
      public sealed class Mapper : IResultErrorMapper<Error, ProblemHttpResult>
      {
        public required string Name { get; init; }
        public ProblemHttpResult Map(Error error) => TypedResults.Problem();
      }
      """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().ContainSingle(diagnostic => diagnostic.Id == "CFEHTTPR004");
    files["LegacyErrorMapperCache.g.cs"].Should().NotContain("new global::Mapper");
  }

  [Fact]
  public void SetsRequiredMembers_constructor_is_supported()
  {
    const string source = """
      using CSharpFunctionalExtensions.HttpResults;
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using System.Diagnostics.CodeAnalysis;

      public sealed class Error;
      public sealed class Mapper : IResultErrorMapper<Error, ProblemHttpResult>
      {
        public required string Name { get; init; }
        [SetsRequiredMembers]
        public Mapper() => Name = "mapper";
        public ProblemHttpResult Map(Error error) => TypedResults.Problem();
      }
      """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().BeEmpty();
    files["LegacyErrorMapperCache.g.cs"].Should().Contain("new global::Mapper()");
  }

  [Fact]
  public void Same_short_mapper_names_get_unique_cache_members()
  {
    const string source = """
      using CSharpFunctionalExtensions.HttpResults;
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;

      namespace First { public sealed class Error; public sealed class Mapper : IResultErrorMapper<Error, ProblemHttpResult> { public ProblemHttpResult Map(Error error) => TypedResults.Problem(); } }
      namespace Second { public sealed class Error; public sealed class Mapper : IResultErrorMapper<Error, ProblemHttpResult> { public ProblemHttpResult Map(Error error) => TypedResults.Problem(); } }
      """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().BeEmpty();
    var cache = files["LegacyErrorMapperCache.g.cs"];
    cache.Should().Contain("Mapper0").And.Contain("Mapper1");
    cache.Should().Contain("new global::First.Mapper()").And.Contain("new global::Second.Mapper()");
  }

  [Fact]
  public void Invalid_mapper_does_not_suppress_independent_generation()
  {
    const string source = """
      using CSharpFunctionalExtensions.HttpResults;
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;

      public sealed class InvalidError;
      public sealed class InvalidMapper(string dependency) : IResultErrorMapper<InvalidError, ProblemHttpResult>
      {
        public ProblemHttpResult Map(InvalidError error) => TypedResults.Problem();
      }
      public sealed class ValidError;
      public sealed class ValidMapper : IServiceResultErrorMapper<ValidError, ProblemHttpResult>
      {
        public ProblemHttpResult Map(ValidError error) => TypedResults.Problem();
      }
      """;

    var (diagnostics, files) = GeneratorTestHelper.RunGeneratorPerFile(source);

    diagnostics.Should().ContainSingle(diagnostic => diagnostic.Id == "CFEHTTPR004");
    files["StringErrorResultExtensions.g.cs"].Should().Contain("ToOkHttpResult");
    files["ResultExtensions.g.cs"].Should().Contain("global::ValidError").And.NotContain("global::InvalidError");
    files["ServiceCollectionExtensions.g.cs"].Should().Contain("TryAddScoped<global::ValidMapper>()");
  }
}

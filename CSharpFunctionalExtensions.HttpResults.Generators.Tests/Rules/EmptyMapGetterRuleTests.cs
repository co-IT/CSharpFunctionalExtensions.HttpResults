using FluentAssertions;
using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests.Rules;

public class EmptyMapGetterRuleTests
{
  [Fact]
  public void TestValidExpressionBodiedGetter()
  {
    var sourceCode = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      public class DocumentCreationError
      {
        public required string DocumentId { get; init; }
      }

      public class DocumentCreationErrorMapper : IResultErrorMapper<DocumentCreationError, Conflict<string>>
      {
          public Func<DocumentCreationError, Conflict<string>> Map => error => TypedResults.Conflict(error.DocumentId);
      }
      """;

    var diagnostics = ResultExtensionsGeneratorTestHelper.RunGenerator(sourceCode);

    diagnostics.Should().BeEmpty();
  }

  [Fact]
  public void TestValidBlockBodiedGetter()
  {
    var sourceCode = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      public class DocumentCreationError
      {
        public required string DocumentId { get; init; }
      }

      public class DocumentCreationErrorMapper : IResultErrorMapper<DocumentCreationError, Conflict<string>>
      {
          public Func<DocumentCreationError, Conflict<string>> Map
          {
              get
              {
                  return error => TypedResults.Conflict(error.DocumentId);
              }
          }
      }
      """;

    var diagnostics = ResultExtensionsGeneratorTestHelper.RunGenerator(sourceCode);

    diagnostics.Should().BeEmpty();
  }

  [Fact]
  public void TestInvalidAutoPropertyGetter()
  {
    var sourceCode = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      public class DocumentCreationError
      {
        public required string DocumentId { get; init; }
      }

      public class DocumentCreationErrorMapper : IResultErrorMapper<DocumentCreationError, Conflict<string>>
      {
        public Func<DocumentCreationError, Conflict<string>> Map {get;}
      }
      """;

    var diagnostics = ResultExtensionsGeneratorTestHelper.RunGenerator(sourceCode).ToList();

    diagnostics.Count.Should().Be(1);

    var diagnostic = diagnostics[0];

    diagnostic.Id.Should().Be("CFEHTTPR003");
    diagnostic.Severity.Should().Be(DiagnosticSeverity.Error);
    diagnostic.GetMessage().Should().Be("Class 'DocumentCreationErrorMapper' must implement the 'Map' property getter");
  }
}

using FluentAssertions;
using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests.Rules;

public class DuplicateMapperRuleTests
{
  [Fact]
  public void TestDuplicateMappers()
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
        public Conflict<string> Map(DocumentCreationError error) => TypedResults.Conflict(error.DocumentId);
      }

      public class DocumentCreationErrorMapper2 : IResultErrorMapper<DocumentCreationError, Conflict<string>>
      {
        public Conflict<string> Map(DocumentCreationError error) => TypedResults.Conflict(error.DocumentId);
      }
      """;

    var diagnostics = ResultExtensionsGeneratorTestHelper.RunGenerator(sourceCode).ToList();

    diagnostics.Count.Should().Be(1);

    var diagnostic = diagnostics[0];

    diagnostic.Id.Should().Be("CFEHTTPR002");
    diagnostic.Severity.Should().Be(DiagnosticSeverity.Error);
    diagnostic.GetMessage().Should().Be("Class 'DocumentCreationError' does have multiple IResultErrorMapper");
  }
}

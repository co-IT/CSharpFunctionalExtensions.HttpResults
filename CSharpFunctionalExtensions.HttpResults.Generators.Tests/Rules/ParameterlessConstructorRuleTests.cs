using FluentAssertions;
using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests.Rules;

public class ParameterlessConstructorRuleTests
{
  [Fact]
  public void TestParameterlessConstructor()
  {
    var sourceCode = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;

      public class DocumentCreationError
      {
        public required string DocumentId { get; init; }
      }

      // Not parameterless --> error
      public class DocumentCreationErrorMapper : IResultErrorMapper<DocumentCreationError, Conflict<string>>
      {
        public DocumentCreationErrorMapper(string foo) { }

        public Conflict<string> Map(DocumentCreationError error) => TypedResults.Conflict(error.DocumentId);
      }

      // Explicit parameterless --> No error
      public class DocumentCreationErrorMapper2 : IResultErrorMapper<string, Conflict<string>>
      {
        public DocumentCreationErrorMapper2() { }

        public Conflict<string> Map(string error) => TypedResults.Conflict(error.DocumentId);
      }

      // Explicit parameterless & with parameters --> No error
      public class DocumentCreationErrorMapper3 : IResultErrorMapper<int, Conflict<string>>
      {
        public DocumentCreationErrorMapper3() { }
        public DocumentCreationErrorMapper3(string foo) { }

        public Conflict<string> Map(int error) => TypedResults.Conflict(error.DocumentId);
      }
      """;

    var (diagnostics, _) = GeneratorTestHelper.RunGenerator(sourceCode);

    var diagnosticsList = diagnostics.ToList();

    diagnosticsList.Should().HaveCount(1);

    var diagnostic = diagnosticsList[0];

    diagnostic.Id.Should().Be("CFEHTTPR004");
    diagnostic.Severity.Should().Be(DiagnosticSeverity.Error);
    diagnostic
      .GetMessage()
      .Should()
      .Be("Class 'DocumentCreationErrorMapper' does not have a parameterless constructor");
  }
}

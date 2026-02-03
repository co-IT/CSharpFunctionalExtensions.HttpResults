using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests;

/// <summary>
///   Tests that verify the source generator correctly handles error types defined in external assemblies.
///   This addresses the issue where short type names were used instead of fully qualified names,
///   causing compilation errors when error types came from referenced assemblies.
/// </summary>
public class ExternalAssemblyErrorTypeTests
{
  [Fact]
  public void GeneratesFullyQualifiedTypeNamesForExternalErrorType_WithToOkHttpResult()
  {
    // Create a fake external assembly with an error type
    var externalErrorTypeCode = """
      namespace MyApp.Infrastructure.Errors;

      public sealed record NotFoundError(string Message);
      """;

    var externalAssemblySyntaxTree = CSharpSyntaxTree.ParseText(externalErrorTypeCode);
    var externalCompilation = CSharpCompilation.Create(
      "ExternalAssembly",
      [externalAssemblySyntaxTree],
      [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
    );

    // Emit the external assembly to get a reference
    using var ms = new MemoryStream();
    var emitResult = externalCompilation.Emit(ms);
    emitResult.Success.Should().BeTrue("External assembly should compile");

    var externalAssemblyReference = MetadataReference.CreateFromStream(new MemoryStream(ms.ToArray()));

    // Create mapper in the main assembly that references the external error type
    var mapperCode = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;
      using MyApp.Infrastructure.Errors;

      namespace MyApp.Api.ErrorMappers;

      public class NotFoundErrorMapper : IResultErrorMapper<NotFoundError, ProblemHttpResult>
      {
          public ProblemHttpResult Map(NotFoundError error) =>
              TypedResults.Problem(
                  statusCode: StatusCodes.Status404NotFound,
                  title: "Not Found",
                  detail: error.Message
              );
      }
      """;

    var (_, generatedSource) = GeneratorTestHelper.RunGenerator(mapperCode, [externalAssemblyReference]);

    generatedSource.Should().Contain("this Result<T,global::MyApp.Infrastructure.Errors.NotFoundError> result");
  }

  [Fact]
  public void GeneratesFullyQualifiedTypeNamesForExternalErrorType_MultipleMappers()
  {
    // Create fake external assemblies with error types
    var externalErrorTypesCode = """
      namespace MyApp.Infrastructure.Errors;

      public sealed record NotFoundError(string Message);
      public sealed record ValidationError(string Message);
      """;

    var externalAssemblySyntaxTree = CSharpSyntaxTree.ParseText(externalErrorTypesCode);
    var externalCompilation = CSharpCompilation.Create(
      "ExternalAssembly",
      [externalAssemblySyntaxTree],
      [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
    );

    using var ms = new MemoryStream();
    var emitResult = externalCompilation.Emit(ms);
    emitResult.Success.Should().BeTrue();

    var externalAssemblyReference = MetadataReference.CreateFromStream(new MemoryStream(ms.ToArray()));

    // Create multiple mappers
    var mappersCode = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;
      using MyApp.Infrastructure.Errors;

      namespace MyApp.Api.ErrorMappers;

      public class NotFoundErrorMapper : IResultErrorMapper<NotFoundError, ProblemHttpResult>
      {
          public ProblemHttpResult Map(NotFoundError error) =>
              TypedResults.Problem(
                  statusCode: StatusCodes.Status404NotFound,
                  title: "Not Found",
                  detail: error.Message
              );
      }

      public class ValidationErrorMapper : IResultErrorMapper<ValidationError, BadRequest<ProblemHttpResult>>
      {
          public BadRequest<ProblemHttpResult> Map(ValidationError error) =>
              TypedResults.BadRequest(
                  TypedResults.Problem(
                      statusCode: StatusCodes.Status400BadRequest,
                      title: "Validation Error",
                      detail: error.Message
                  )
              );
      }
      """;

    var (_, generatedSource) = GeneratorTestHelper.RunGenerator(mappersCode, [externalAssemblyReference]);

    generatedSource.Should().Contain("this Result<T,global::MyApp.Infrastructure.Errors.ValidationError");
    generatedSource.Should().Contain("this Result<T,global::MyApp.Infrastructure.Errors.NotFoundError");
  }

  [Fact]
  public void NoCompilationErrorsWithExternalErrorType()
  {
    // Create external assembly
    var externalErrorTypeCode = """
      namespace MyApp.Infrastructure.Errors;

      public sealed record NotFoundError(string Message);
      """;

    var externalAssemblySyntaxTree = CSharpSyntaxTree.ParseText(externalErrorTypeCode);
    var externalCompilation = CSharpCompilation.Create(
      "ExternalAssembly",
      [externalAssemblySyntaxTree],
      [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
      new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
    );

    using var ms = new MemoryStream();
    var emitResult = externalCompilation.Emit(ms);
    emitResult.Success.Should().BeTrue();

    var externalAssemblyReference = MetadataReference.CreateFromStream(new MemoryStream(ms.ToArray()));

    var mapperCode = """
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using CSharpFunctionalExtensions.HttpResults;
      using MyApp.Infrastructure.Errors;

      namespace MyApp.Api.ErrorMappers;

      public class NotFoundErrorMapper : IResultErrorMapper<NotFoundError, ProblemHttpResult>
      {
          public ProblemHttpResult Map(NotFoundError error) =>
              TypedResults.Problem(
                  statusCode: StatusCodes.Status404NotFound,
                  title: "Not Found",
                  detail: error.Message
              );
      }
      """;

    var (diagnostics, generatedSource) = GeneratorTestHelper.RunGenerator(mapperCode, [externalAssemblyReference]);

    diagnostics.Should().BeEmpty("Should not report diagnostics about NotFoundError being unresolved");
    generatedSource.Should().NotBeNullOrEmpty("Should generate extension methods");
  }
}

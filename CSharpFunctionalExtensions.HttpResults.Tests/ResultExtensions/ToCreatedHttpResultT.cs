using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToCreatedHttpResultT
{
  [Fact]
  public void ResultT_Success_can_be_mapped_to_CreatedHttpResult()
  {
    var document = new Document { DocumentId = Guid.NewGuid().ToString() };
    Func<Document, Uri> uri = x => new Uri($"http://localhost/documents/{x.DocumentId}");

    var result = Result.Success(document).ToCreatedHttpResult(uri).Result as Created<Document>;

    result!.StatusCode.Should().Be(201);
    result!.Value.Should().Be(document);
    result!.Location.Should().Be(uri(document).AbsoluteUri);
  }

  [Fact]
  public async Task ResultT_Success_can_be_mapped_to_CreatedHttpResult_Async()
  {
    var document = new Document { DocumentId = Guid.NewGuid().ToString() };
    Func<Document, Uri> uri = x => new Uri($"http://localhost/documents/{x.DocumentId}");

    var result = (await Task.FromResult(Result.Success(document)).ToCreatedHttpResult(uri)).Result as Created<Document>;

    result!.StatusCode.Should().Be(201);
    result!.Value.Should().Be(document);
    result!.Location.Should().Be(uri(document).AbsoluteUri);
  }

  [Fact]
  public void ResultT_Failure_can_be_mapped_to_CreatedHttpResult()
  {
    var error = "Error";

    var result = Result.Failure<Document>(error).ToCreatedHttpResult().Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultT_Failure_can_be_mapped_to_CreatedHttpResult_Async()
  {
    var error = "Error";

    var result =
      (await Task.FromResult(Result.Failure<Document>(error)).ToCreatedHttpResult()).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultT_Failure_StatusCode_can_be_changed()
  {
    var statusCode = 418;
    var error = "Error";

    var result =
      Result.Failure<Document>(error).ToCreatedHttpResult(failureStatusCode: statusCode).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ProblemDetails.Status.Should().Be(statusCode);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultT_Failure_StatusCode_can_be_changed_Async()
  {
    var statusCode = 418;
    var error = "Error";

    var result =
      (await Task.FromResult(Result.Failure<Document>(error)).ToCreatedHttpResult(failureStatusCode: statusCode)).Result
      as ProblemHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ProblemDetails.Status.Should().Be(statusCode);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultT_Failure_ProblemDetails_can_be_customized()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result =
      Result
        .Failure<Document>(error)
        .ToCreatedHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
        .Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be(customTitle);
  }

  [Fact]
  public async Task ResultT_Failure_ProblemDetails_can_be_customized_Async()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result =
      (
        await Task.FromResult(Result.Failure<Document>(error))
          .ToCreatedHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
      ).Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be(customTitle);
  }
}

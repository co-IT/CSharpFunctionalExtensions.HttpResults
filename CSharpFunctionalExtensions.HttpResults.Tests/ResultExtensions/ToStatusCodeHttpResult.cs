using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToStatusCodeHttpResult
{
  [Fact]
  public void Result_Success_can_be_mapped_to_StatusCodeHttpResult()
  {
    var result = Result.Success().ToStatusCodeHttpResult().Result as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task Result_Success_can_be_mapped_to_StatusCodeHttpResult_Async()
  {
    var result = (await Task.FromResult(Result.Success()).ToStatusCodeHttpResult()).Result as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(204);
  }

  [Fact]
  public void Result_Success_StatusCode_can_be_changed()
  {
    var statusCode = 210;

    var result = Result.Success().ToStatusCodeHttpResult(statusCode).Result as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(statusCode);
  }

  [Fact]
  public async Task Result_Success_StatusCode_can_be_changed_Async()
  {
    var statusCode = 210;

    var result =
      (await Task.FromResult(Result.Success()).ToStatusCodeHttpResult(statusCode)).Result as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(statusCode);
  }

  [Fact]
  public void Result_Failure_can_be_mapped_to_StatusCodeHttpResult()
  {
    var error = "Error";

    var result = Result.Failure(error).ToStatusCodeHttpResult().Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task Result_Failure_can_be_mapped_to_StatusCodeHttpResult_Async()
  {
    var error = "Error";

    var result = (await Task.FromResult(Result.Failure(error)).ToStatusCodeHttpResult()).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void Result_Failure_StatusCode_can_be_changed()
  {
    var statusCode = 418;
    var error = "Error";

    var result =
      Result.Failure(error).ToStatusCodeHttpResult(failureStatusCode: statusCode).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ProblemDetails.Status.Should().Be(statusCode);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task Result_Failure_StatusCode_can_be_changed_Async()
  {
    var statusCode = 418;
    var error = "Error";

    var result =
      (await Task.FromResult(Result.Failure(error)).ToStatusCodeHttpResult(failureStatusCode: statusCode)).Result
      as ProblemHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ProblemDetails.Status.Should().Be(statusCode);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void Result_Failure_ProblemDetails_can_be_customized()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result =
      Result
        .Failure(error)
        .ToStatusCodeHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
        .Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be(customTitle);
  }

  [Fact]
  public async Task Result_Failure_ProblemDetails_can_be_customized_Async()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result =
      (
        await Task.FromResult(Result.Failure(error))
          .ToStatusCodeHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
      ).Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be(customTitle);
  }
}

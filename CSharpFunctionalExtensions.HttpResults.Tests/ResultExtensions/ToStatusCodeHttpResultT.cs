using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToStatusCodeHttpResultT
{
  [Fact]
  public void ResultT_Success_can_be_mapped_to_StatusCodeHttpResult()
  {
    var value = "foo";

    var result = Result.Success(value).ToStatusCodeHttpResult().Result as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task ResultT_Success_can_be_mapped_to_StatusCodeHttpResult_Async()
  {
    var value = "foo";

    var result = (await Task.FromResult(Result.Success(value)).ToStatusCodeHttpResult()).Result as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(204);
  }

  [Fact]
  public void ResultT_Success_StatusCode_can_be_changed()
  {
    var statusCode = 210;
    var value = "foo";

    var result = Result.Success(value).ToStatusCodeHttpResult(statusCode).Result as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(statusCode);
  }

  [Fact]
  public async Task ResultT_Success_StatusCode_can_be_changed_Async()
  {
    var statusCode = 210;
    var value = "foo";

    var result =
      (await Task.FromResult(Result.Success(value)).ToStatusCodeHttpResult(statusCode)).Result as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(statusCode);
  }

  [Fact]
  public void ResultT_Failure_can_be_mapped_to_HttpResult()
  {
    var error = "Error";

    var result = Result.Failure<string>(error).ToStatusCodeHttpResult().Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultT_Failure_can_be_mapped_to_StatusCodeHttpResult_Async()
  {
    var error = "Error";

    var result =
      (await Task.FromResult(Result.Failure<string>(error)).ToStatusCodeHttpResult()).Result as ProblemHttpResult;

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
      Result.Failure<string>(error).ToStatusCodeHttpResult(failureStatusCode: statusCode).Result as ProblemHttpResult;

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
      (
        await Task.FromResult(Result.Failure<string>(error)).ToStatusCodeHttpResult(failureStatusCode: statusCode)
      ).Result as ProblemHttpResult;

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
        .Failure<string>(error)
        .ToStatusCodeHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
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
        await Task.FromResult(Result.Failure<string>(error))
          .ToStatusCodeHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
      ).Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be(customTitle);
  }
}

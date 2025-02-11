using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToOkHttpResultT
{
  [Fact]
  public void ResultT_Success_can_be_mapped_to_OkHttpResult()
  {
    var value = "foo";

    var result = Result.Success(value).ToOkHttpResult().Result as Ok<string>;

    result!.StatusCode.Should().Be(200);
    result!.Value.Should().Be(value);
  }

  [Fact]
  public async Task ResultT_Success_can_be_mapped_to_OkHttpResult_Async()
  {
    var value = "foo";

    var result = (await Task.FromResult(Result.Success(value)).ToOkHttpResult()).Result as Ok<string>;

    result!.StatusCode.Should().Be(200);
    result!.Value.Should().Be(value);
  }

  [Fact]
  public void ResultT_Failure_can_be_mapped_to_OkHttpResult()
  {
    var error = "Error";

    var result = Result.Failure<string>(error).ToOkHttpResult().Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultT_Failure_can_be_mapped_to_OkHttpResult_Async()
  {
    var error = "Error";

    var result = (await Task.FromResult(Result.Failure<string>(error)).ToOkHttpResult()).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultT_Failure_StatusCode_can_be_changed()
  {
    var statusCode = 418;
    var error = "Error";

    var result = Result.Failure<string>(error).ToOkHttpResult(statusCode).Result as ProblemHttpResult;

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
      (await Task.FromResult(Result.Failure<string>(error)).ToOkHttpResult(statusCode)).Result as ProblemHttpResult;

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
        .ToOkHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
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
          .ToOkHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
      ).Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be(customTitle);
  }
}

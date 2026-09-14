using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToNoContentHttpResultT
{
  [Fact]
  public void ResultT_Success_can_be_mapped_to_NoContentHttpResult()
  {
    var value = "foo";

    var result = Assert.IsType<NoContent>(Result.Success(value).ToNoContentHttpResult().Result);

    result.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task ResultT_Success_can_be_mapped_to_NoContentHttpResult_Async()
  {
    var value = "foo";

    var result = Assert.IsType<NoContent>(
      (await Task.FromResult(Result.Success(value)).ToNoContentHttpResult()).Result
    );

    result.StatusCode.Should().Be(204);
  }

  [Fact]
  public void ResultT_Failure_can_be_mapped_to_HttpResult()
  {
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(Result.Failure<string>(error).ToNoContentHttpResult().Result);

    result.StatusCode.Should().Be(400);
    result.ProblemDetails.Status.Should().Be(400);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultT_Failure_can_be_mapped_to_NoContentHttpResult_Async()
  {
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      (await Task.FromResult(Result.Failure<string>(error)).ToNoContentHttpResult()).Result
    );

    result.StatusCode.Should().Be(400);
    result.ProblemDetails.Status.Should().Be(400);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultT_Failure_StatusCode_can_be_changed()
  {
    var statusCode = 418;
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      Result.Failure<string>(error).ToNoContentHttpResult(statusCode).Result
    );

    result.StatusCode.Should().Be(statusCode);
    result.ProblemDetails.Status.Should().Be(statusCode);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultT_Failure_StatusCode_can_be_changed_Async()
  {
    var statusCode = 418;
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      (await Task.FromResult(Result.Failure<string>(error)).ToNoContentHttpResult(statusCode)).Result
    );

    result.StatusCode.Should().Be(statusCode);
    result.ProblemDetails.Status.Should().Be(statusCode);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultT_Failure_ProblemDetails_can_be_customized()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result = Assert.IsType<ProblemHttpResult>(
      Result
        .Failure<string>(error)
        .ToNoContentHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
        .Result
    );

    result.ProblemDetails.Title.Should().Be(customTitle);
  }

  [Fact]
  public async Task ResultT_Failure_ProblemDetails_can_be_customized_Async()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result = Assert.IsType<ProblemHttpResult>(
      (
        await Task.FromResult(Result.Failure<string>(error))
          .ToNoContentHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
      ).Result
    );

    result.ProblemDetails.Title.Should().Be(customTitle);
  }
}

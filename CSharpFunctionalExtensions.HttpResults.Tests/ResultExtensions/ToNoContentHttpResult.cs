using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToNoContentHttpResult
{
  [Fact]
  public void Result_Success_can_be_mapped_to_NoContentHttpResult()
  {
    var result = Assert.IsType<NoContent>(Result.Success().ToNoContentHttpResult().Result);

    result.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task Result_Success_can_be_mapped_to_NoContentHttpResult_Async()
  {
    var result = Assert.IsType<NoContent>((await Task.FromResult(Result.Success()).ToNoContentHttpResult()).Result);

    result.StatusCode.Should().Be(204);
  }

  [Fact]
  public void Result_Failure_can_be_mapped_to_NoContentHttpResult()
  {
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(Result.Failure(error).ToNoContentHttpResult().Result);

    result.StatusCode.Should().Be(400);
    result.ProblemDetails.Status.Should().Be(400);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task Result_Failure_can_be_mapped_to_NoContentHttpResult_Async()
  {
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      (await Task.FromResult(Result.Failure(error)).ToNoContentHttpResult()).Result
    );

    result.StatusCode.Should().Be(400);
    result.ProblemDetails.Status.Should().Be(400);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void Result_Failure_StatusCode_can_be_changed()
  {
    var statusCode = 418;
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(Result.Failure(error).ToNoContentHttpResult(statusCode).Result);

    result.StatusCode.Should().Be(statusCode);
    result.ProblemDetails.Status.Should().Be(statusCode);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task Result_Failure_StatusCode_can_be_changed_Async()
  {
    var statusCode = 418;
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      (await Task.FromResult(Result.Failure(error)).ToNoContentHttpResult(statusCode)).Result
    );

    result.StatusCode.Should().Be(statusCode);
    result.ProblemDetails.Status.Should().Be(statusCode);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void Result_Failure_ProblemDetails_can_be_customized()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result = Assert.IsType<ProblemHttpResult>(
      Result
        .Failure(error)
        .ToNoContentHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
        .Result
    );

    result.ProblemDetails.Title.Should().Be(customTitle);
  }

  [Fact]
  public async Task Result_Failure_ProblemDetails_can_be_customized_Async()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result = Assert.IsType<ProblemHttpResult>(
      (
        await Task.FromResult(Result.Failure(error))
          .ToNoContentHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
      ).Result
    );

    result.ProblemDetails.Title.Should().Be(customTitle);
  }
}

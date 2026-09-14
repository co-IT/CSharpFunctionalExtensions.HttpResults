using System.Text;
using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToContentHttpResultString
{
  [Fact]
  public void ResultString_Success_can_be_mapped_to_ContentHttpResult()
  {
    var value = "foo";

    var result = Assert.IsType<ContentHttpResult>(Result.Success(value).ToContentHttpResult().Result);

    result.ResponseContent.Should().Be(value);
  }

  [Fact]
  public async Task ResultString_Success_can_be_mapped_to_ContentHttpResult_Async()
  {
    var value = "foo";

    var result = Assert.IsType<ContentHttpResult>(
      (await Task.FromResult(Result.Success(value)).ToContentHttpResult()).Result
    );

    result.ResponseContent.Should().Be(value);
  }

  [Fact]
  public void ResultString_Failure_can_be_mapped_to_ContentHttpResult()
  {
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(Result.Failure<string>(error).ToContentHttpResult().Result);

    result.StatusCode.Should().Be(400);
    result.ProblemDetails.Status.Should().Be(400);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultString_Failure_can_be_mapped_to_ContentHttpResult_Async()
  {
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      (await Task.FromResult(Result.Failure<string>(error)).ToContentHttpResult()).Result
    );

    result.StatusCode.Should().Be(400);
    result.ProblemDetails.Status.Should().Be(400);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultString_Failure_StatusCode_can_be_changed()
  {
    var statusCode = 418;
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      Result.Failure<string>(error).ToContentHttpResult(null, null, null, statusCode).Result
    );

    result.StatusCode.Should().Be(statusCode);
    result.ProblemDetails.Status.Should().Be(statusCode);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultString_Failure_StatusCode_can_be_changed_Async()
  {
    var statusCode = 418;
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      (await Task.FromResult(Result.Failure<string>(error)).ToContentHttpResult(null, null, null, statusCode)).Result
    );

    result.StatusCode.Should().Be(statusCode);
    result.ProblemDetails.Status.Should().Be(statusCode);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultString_Success_can_be_mapped_to_ContentHttpResult_with_configuration()
  {
    var value = "foo";
    var contentType = "text/csv";
    var encoding = Encoding.ASCII;
    var statusCode = 218;

    var result = Assert.IsType<ContentHttpResult>(
      Result.Success(value).ToContentHttpResult(contentType, encoding, statusCode).Result
    );

    result.StatusCode.Should().Be(statusCode);
    result.ContentType!.Split("; ")[0].Should().Be(contentType);
    result.ContentType!.Split("; ")[1].Replace("charset=", string.Empty).Should().Be(encoding.HeaderName);
    result.ResponseContent.Should().Be(value);
  }

  [Fact]
  public async Task ResultString_Success_can_be_mapped_to_ContentHttpResult_with_configuration_Async()
  {
    var value = "foo";
    var contentType = "text/csv";
    var encoding = Encoding.ASCII;
    var statusCode = 218;

    var result = Assert.IsType<ContentHttpResult>(
      (await Task.FromResult(Result.Success(value)).ToContentHttpResult(contentType, encoding, statusCode)).Result
    );

    result.StatusCode.Should().Be(statusCode);
    result.ContentType!.Split("; ")[0].Should().Be(contentType);
    result.ContentType!.Split("; ")[1].Replace("charset=", string.Empty).Should().Be(encoding.HeaderName);
    result.ResponseContent.Should().Be(value);
  }

  [Fact]
  public void ResultString_Failure_ProblemDetails_can_be_customized()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result = Assert.IsType<ProblemHttpResult>(
      Result
        .Failure<string>(error)
        .ToContentHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
        .Result
    );

    result.ProblemDetails.Title.Should().Be(customTitle);
  }

  [Fact]
  public async Task ResultString_Failure_ProblemDetails_can_be_customized_Async()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result = Assert.IsType<ProblemHttpResult>(
      (
        await Task.FromResult(Result.Failure<string>(error))
          .ToContentHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
      ).Result
    );

    result.ProblemDetails.Title.Should().Be(customTitle);
  }
}

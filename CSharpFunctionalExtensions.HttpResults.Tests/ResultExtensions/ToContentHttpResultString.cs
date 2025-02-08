using System.Text;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToContentHttpResultString
{
  [Fact]
  public void ResultString_Success_can_be_mapped_to_ContentHttpResult()
  {
    var value = "foo";

    var result = Result.Success(value).ToContentHttpResult().Result as ContentHttpResult;

    result!.ResponseContent.Should().Be(value);
  }

  [Fact]
  public async Task ResultString_Success_can_be_mapped_to_ContentHttpResult_Async()
  {
    var value = "foo";

    var result = (await Task.FromResult(Result.Success(value)).ToContentHttpResult()).Result as ContentHttpResult;

    result!.ResponseContent.Should().Be(value);
  }

  [Fact]
  public void ResultString_Failure_can_be_mapped_to_ContentHttpResult()
  {
    var error = "Error";

    var result = Result.Failure<string>(error).ToContentHttpResult().Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultString_Failure_can_be_mapped_to_ContentHttpResult_Async()
  {
    var error = "Error";

    var result =
      (await Task.FromResult(Result.Failure<string>(error)).ToContentHttpResult()).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultString_Failure_StatusCode_can_be_changed()
  {
    var statusCode = 418;
    var error = "Error";

    var result =
      Result.Failure<string>(error).ToContentHttpResult(null, null, null, statusCode).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ProblemDetails.Status.Should().Be(statusCode);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultString_Failure_StatusCode_can_be_changed_Async()
  {
    var statusCode = 418;
    var error = "Error";

    var result =
      (await Task.FromResult(Result.Failure<string>(error)).ToContentHttpResult(null, null, null, statusCode)).Result
      as ProblemHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ProblemDetails.Status.Should().Be(statusCode);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultString_Success_can_be_mapped_to_ContentHttpResult_with_configuration()
  {
    var value = "foo";
    var contentType = "text/csv";
    var encoding = Encoding.ASCII;
    var statusCode = 218;

    var result =
      Result.Success(value).ToContentHttpResult(contentType, encoding, statusCode).Result as ContentHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ContentType!.Split("; ")[0].Should().Be(contentType);
    result!.ContentType!.Split("; ")[1].Replace("charset=", string.Empty).Should().Be(encoding.HeaderName);
    result!.ResponseContent.Should().Be(value);
  }

  [Fact]
  public async Task ResultString_Success_can_be_mapped_to_ContentHttpResult_with_configuration_Async()
  {
    var value = "foo";
    var contentType = "text/csv";
    var encoding = Encoding.ASCII;
    var statusCode = 218;

    var result =
      (await Task.FromResult(Result.Success(value)).ToContentHttpResult(contentType, encoding, statusCode)).Result
      as ContentHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ContentType!.Split("; ")[0].Should().Be(contentType);
    result!.ContentType!.Split("; ")[1].Replace("charset=", string.Empty).Should().Be(encoding.HeaderName);
    result!.ResponseContent.Should().Be(value);
  }
}

using System.Text;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToContentHttpResultStringE
{
  [Fact]
  public void ResultStringE_Success_can_be_mapped_to_ContentHttpResult()
  {
    var value = "foo";

    var result = Result.Success<string, DocumentMissingError>(value).ToContentHttpResult().Result as ContentHttpResult;

    result!.ResponseContent.Should().Be(value);
  }

  [Fact]
  public async Task ResultStringE_Success_can_be_mapped_to_ContentHttpResult_Async()
  {
    var value = "foo";

    var result =
      (await Task.FromResult(Result.Success<string, DocumentMissingError>(value)).ToContentHttpResult()).Result
      as ContentHttpResult;

    result!.ResponseContent.Should().Be(value);
  }

  [Fact]
  public void ResultStringE_Failure_can_be_mapped_to_ContentHttpResult()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = Result.Failure<string, DocumentMissingError>(error).ToContentHttpResult().Result as NotFound<string>;

    result!.StatusCode.Should().Be(404);
    result!.Value.Should().Be(error.DocumentId);
  }

  [Fact]
  public async Task ResultStringE_Failure_can_be_mapped_to_ContentHttpResult_Async()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result =
      (await Task.FromResult(Result.Failure<string, DocumentMissingError>(error)).ToContentHttpResult()).Result
      as NotFound<string>;

    result!.StatusCode.Should().Be(404);
    result!.Value.Should().Be(error.DocumentId);
  }

  [Fact]
  public void ResultStringE_Success_can_be_mapped_to_ContentHttpResult_with_configuration()
  {
    var value = "foo";
    var contentType = "text/csv";
    var encoding = Encoding.ASCII;
    var statusCode = 218;

    var result =
      Result.Success<string, DocumentMissingError>(value).ToContentHttpResult(contentType, encoding, statusCode).Result
      as ContentHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ContentType!.Split("; ")[0].Should().Be(contentType);
    result!.ContentType!.Split("; ")[1].Replace("charset=", string.Empty).Should().Be(encoding.HeaderName);
    result!.ResponseContent.Should().Be(value);
  }

  [Fact]
  public async Task ResultStringE_Success_can_be_mapped_to_ContentHttpResult_with_configuration_Async()
  {
    var value = "foo";
    var contentType = "text/csv";
    var encoding = Encoding.ASCII;
    var statusCode = 218;

    var result =
      (
        await Task.FromResult(Result.Success<string, DocumentMissingError>(value))
          .ToContentHttpResult(contentType, encoding, statusCode)
      ).Result as ContentHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ContentType!.Split("; ")[0].Should().Be(contentType);
    result!.ContentType!.Split("; ")[1].Replace("charset=", string.Empty).Should().Be(encoding.HeaderName);
    result!.ResponseContent.Should().Be(value);
  }
}

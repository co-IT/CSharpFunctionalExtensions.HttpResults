using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToNoContentHttpResultTE
{
  [Fact]
  public void ResultTE_Success_can_be_mapped_to_NoContentHttpResult()
  {
    var value = "foo";

    var result = Assert.IsType<NoContent>(
      Result.Success<string, DocumentMissingError>(value).ToNoContentHttpResult().Result
    );

    result.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task ResultTE_Success_can_be_mapped_to_NoContentHttpResult_Async()
  {
    var value = "foo";

    var result = Assert.IsType<NoContent>(
      (await Task.FromResult(Result.Success<string, DocumentMissingError>(value)).ToNoContentHttpResult()).Result
    );

    result.StatusCode.Should().Be(204);
  }

  [Fact]
  public void ResultTE_Failure_can_be_mapped_to_HttpResult()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = Assert.IsType<NotFound<string>>(
      Result.Failure<string, DocumentMissingError>(error).ToNoContentHttpResult().Result
    );

    result.StatusCode.Should().Be(404);
    result.Value.Should().Be(error.DocumentId);
  }

  [Fact]
  public async Task ResultTE_Failure_can_be_mapped_to_NoContentHttpResult_Async()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = Assert.IsType<NotFound<string>>(
      (await Task.FromResult(Result.Failure<string, DocumentMissingError>(error)).ToNoContentHttpResult()).Result
    );

    result.StatusCode.Should().Be(404);
    result.Value.Should().Be(error.DocumentId);
  }
}

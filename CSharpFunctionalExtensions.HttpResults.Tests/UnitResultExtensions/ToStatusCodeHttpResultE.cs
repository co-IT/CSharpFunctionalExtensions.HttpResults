using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.UnitResultExtensions;

public class ToStatusCodeHttpResultE
{
  [Fact]
  public void UnitResultE_Success_can_be_mapped_to_StatusCodeHttpResult()
  {
    var result = UnitResult.Success<DocumentMissingError>().ToStatusCodeHttpResult().Result as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task UnitResultE_Success_can_be_mapped_to_StatusCodeHttpResult_Async()
  {
    var result =
      (await Task.FromResult(UnitResult.Success<DocumentMissingError>()).ToStatusCodeHttpResult()).Result
      as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(204);
  }

  [Fact]
  public void UnitResultE_Success_StatusCode_can_be_changed()
  {
    var statusCode = 210;

    var result =
      UnitResult.Success<DocumentMissingError>().ToStatusCodeHttpResult(statusCode).Result as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(statusCode);
  }

  [Fact]
  public async Task UnitResultE_Success_StatusCode_can_be_changed_Async()
  {
    var statusCode = 210;

    var result =
      (await Task.FromResult(UnitResult.Success<DocumentMissingError>()).ToStatusCodeHttpResult(statusCode)).Result
      as StatusCodeHttpResult;

    result!.StatusCode.Should().Be(statusCode);
  }

  [Fact]
  public void UnitResultE_Failure_can_be_mapped_to_StatusCodeHttpResult()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = UnitResult.Failure(error).ToStatusCodeHttpResult().Result as NotFound<string>;

    result!.StatusCode.Should().Be(404);
    result!.Value.Should().Be(error.DocumentId);
  }

  [Fact]
  public async Task UnitResultE_Failure_can_be_mapped_to_StatusCodeHttpResult_Async()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = (await Task.FromResult(UnitResult.Failure(error)).ToStatusCodeHttpResult()).Result as NotFound<string>;

    result!.StatusCode.Should().Be(404);
    result!.Value.Should().Be(error.DocumentId);
  }
}

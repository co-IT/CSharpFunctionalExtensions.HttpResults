using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.UnitResultExtensions;

public class ToNoContentHttpResultE
{
  [Fact]
  public void UnitResultE_Success_can_be_mapped_to_NoContentHttpResult()
  {
    var result = Assert.IsType<NoContent>(UnitResult.Success<DocumentMissingError>().ToNoContentHttpResult().Result);

    result.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task UnitResultE_Success_can_be_mapped_to_NoContentHttpResult_Async()
  {
    var result = Assert.IsType<NoContent>(
      (await Task.FromResult(UnitResult.Success<DocumentMissingError>()).ToNoContentHttpResult()).Result
    );

    result.StatusCode.Should().Be(204);
  }

  [Fact]
  public void UnitResultE_Failure_can_be_mapped_to_NoContentHttpResult()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = Assert.IsType<NotFound<string>>(UnitResult.Failure(error).ToNoContentHttpResult().Result);

    result.StatusCode.Should().Be(404);
    result.Value.Should().Be(error.DocumentId);
  }

  [Fact]
  public async Task UnitResultE_Failure_can_be_mapped_to_NoContentHttpResult_Async()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = Assert.IsType<NotFound<string>>(
      (await Task.FromResult(UnitResult.Failure(error)).ToNoContentHttpResult()).Result
    );

    result.StatusCode.Should().Be(404);
    result.Value.Should().Be(error.DocumentId);
  }
}

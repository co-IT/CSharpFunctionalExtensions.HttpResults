﻿using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.UnitResultExtensions;

public class ToNoContentHttpResultE
{
  [Fact]
  public void UnitResultE_Success_can_be_mapped_to_NoContentHttpResult()
  {
    var result = UnitResult.Success<DocumentMissingError>().ToNoContentHttpResult().Result as NoContent;

    result!.StatusCode.Should().Be(204);
  }

  [Fact]
  public async Task UnitResultE_Success_can_be_mapped_to_NoContentHttpResult_Async()
  {
    var result =
      (await Task.FromResult(UnitResult.Success<DocumentMissingError>()).ToNoContentHttpResult()).Result as NoContent;

    result!.StatusCode.Should().Be(204);
  }

  [Fact]
  public void UnitResultE_Failure_can_be_mapped_to_NoContentHttpResult()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = UnitResult.Failure(error).ToNoContentHttpResult().Result as NotFound<string>;

    result!.StatusCode.Should().Be(404);
    result!.Value.Should().Be(error.DocumentId);
  }

  [Fact]
  public async Task UnitResultE_Failure_can_be_mapped_to_NoContentHttpResult_Async()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = (await Task.FromResult(UnitResult.Failure(error)).ToNoContentHttpResult()).Result as NotFound<string>;

    result!.StatusCode.Should().Be(404);
    result!.Value.Should().Be(error.DocumentId);
  }
}

using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToCreatedHttpResultTE
{
  [Fact]
  public void ResultTE_Success_can_be_mapped_to_CreatedHttpResult()
  {
    var document = new Document { DocumentId = Guid.NewGuid().ToString() };
    Func<Document, Uri> uri = x => new Uri($"http://localhost/documents/{x.DocumentId}");

    var result =
      Result.Success<Document, DocumentCreationError>(document).ToCreatedHttpResult(uri).Result as Created<Document>;

    result!.StatusCode.Should().Be(201);
    result!.Value.Should().Be(document);
    result!.Location.Should().Be(uri(document).AbsoluteUri);
  }

  [Fact]
  public async Task ResultTE_Success_can_be_mapped_to_CreatedHttpResult_Async()
  {
    var document = new Document { DocumentId = Guid.NewGuid().ToString() };
    Func<Document, Uri> uri = x => new Uri($"http://localhost/documents/{x.DocumentId}");

    var result =
      (await Task.FromResult(Result.Success<Document, DocumentCreationError>(document)).ToCreatedHttpResult(uri)).Result
      as Created<Document>;

    result!.StatusCode.Should().Be(201);
    result!.Value.Should().Be(document);
    result!.Location.Should().Be(uri(document).AbsoluteUri);
  }

  [Fact]
  public void ResultTE_Failure_can_be_mapped_to_CreatedHttpResult()
  {
    var error = new DocumentCreationError { DocumentId = Guid.NewGuid().ToString() };

    var result =
      Result.Failure<Document, DocumentCreationError>(error).ToCreatedHttpResult().Result as Conflict<string>;

    result!.StatusCode.Should().Be(409);
    result!.Value.Should().Be(error.DocumentId);
  }

  [Fact]
  public async Task ResultTE_Failure_can_be_mapped_to_CreatedHttpResult_Async()
  {
    var error = new DocumentCreationError { DocumentId = Guid.NewGuid().ToString() };

    var result =
      (await Task.FromResult(Result.Failure<Document, DocumentCreationError>(error)).ToCreatedHttpResult()).Result
      as Conflict<string>;

    result!.StatusCode.Should().Be(409);
    result!.Value.Should().Be(error.DocumentId);
  }
}

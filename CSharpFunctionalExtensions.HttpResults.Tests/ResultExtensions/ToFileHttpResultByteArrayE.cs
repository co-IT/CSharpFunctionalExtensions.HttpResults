using System.Net.Mime;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Net.Http.Headers;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToFileHttpResultByteArrayE
{
  [Fact]
  public void ResultByteArrayE_Success_can_be_mapped_to_FileHttpResult()
  {
    var value = "foo"u8.ToArray();
    var contentType = MediaTypeNames.Text.Plain;
    var fileDownloadName = "foo.txt";
    var lastModified = DateTimeOffset.Now;
    var entityTag = new EntityTagHeaderValue("\"fooETag\"");
    ;
    var enableRangeProcessing = true;

    var result =
      Result
        .Success<byte[], DocumentMissingError>(value)
        .ToFileHttpResult(contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing)
        .Result as FileContentHttpResult;

    result!.FileContents.ToArray().Should().BeEquivalentTo(value);
    result!.LastModified.Should().Be(lastModified);
    result!.FileDownloadName.Should().Be(fileDownloadName);
    result!.FileLength.Should().Be(value.Length);
    result!.ContentType.Should().Be(contentType);
    result!.EnableRangeProcessing.Should().Be(enableRangeProcessing);
    result!.EntityTag.Should().Be(entityTag);
  }

  [Fact]
  public async Task ResultByteArrayE_Success_can_be_mapped_to_FileHttpResult_Async()
  {
    var value = "foo"u8.ToArray();
    var contentType = MediaTypeNames.Text.Plain;
    var fileDownloadName = "foo.txt";
    var lastModified = DateTimeOffset.Now;
    var entityTag = new EntityTagHeaderValue("\"fooETag\"");
    ;
    var enableRangeProcessing = true;

    var result =
      (
        await Task.FromResult(Result.Success<byte[], DocumentMissingError>(value))
          .ToFileHttpResult(contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing)
      ).Result as FileContentHttpResult;

    result!.FileContents.ToArray().Should().BeEquivalentTo(value);
    result!.LastModified.Should().Be(lastModified);
    result!.FileDownloadName.Should().Be(fileDownloadName);
    result!.FileLength.Should().Be(value.Length);
    result!.ContentType.Should().Be(contentType);
    result!.EnableRangeProcessing.Should().Be(enableRangeProcessing);
    result!.EntityTag.Should().Be(entityTag);
  }

  [Fact]
  public void ResultByteArrayE_Failure_can_be_mapped_to_FileHttpResult()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = Result.Failure<byte[], DocumentMissingError>(error).ToFileHttpResult().Result as NotFound<string>;

    result!.StatusCode.Should().Be(404);
    result!.Value.Should().Be(error.DocumentId);
  }

  [Fact]
  public async Task ResultByteArrayE_Failure_can_be_mapped_to_FileHttpResult_Async()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result =
      (await Task.FromResult(Result.Failure<byte[], DocumentMissingError>(error)).ToFileHttpResult()).Result
      as NotFound<string>;

    result!.StatusCode.Should().Be(404);
    result!.Value.Should().Be(error.DocumentId);
  }
}

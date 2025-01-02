using System.Net.Mime;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Net.Http.Headers;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToFileStreamHttpResultStream
{
  [Fact]
  public void ResultStream_Success_can_be_mapped_to_FileStreamHttpResult()
  {
    var value = "foo"u8.ToArray();
    var stream = new MemoryStream();
    stream.Write(value);
    var contentType = MediaTypeNames.Text.Plain;
    var fileDownloadName = "foo.txt";
    var lastModified = DateTimeOffset.Now;
    var entityTag = new EntityTagHeaderValue("\"fooETag\"");
    ;
    var enableRangeProcessing = true;

    var result =
      Result
        .Success(stream)
        .ToFileStreamHttpResult(contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing)
        .Result as FileStreamHttpResult;

    stream = new MemoryStream();
    result!.FileStream.Position = 0;
    result!.FileStream.CopyTo(stream);
    stream.ToArray().Should().BeEquivalentTo(value);
    result!.LastModified.Should().Be(lastModified);
    result!.FileDownloadName.Should().Be(fileDownloadName);
    result!.FileLength.Should().Be(value.Length);
    result!.ContentType.Should().Be(contentType);
    result!.EnableRangeProcessing.Should().Be(enableRangeProcessing);
    result!.EntityTag.Should().Be(entityTag);
  }

  [Fact]
  public async Task ResultStream_Success_can_be_mapped_to_FileStreamHttpResult_Async()
  {
    var value = "foo"u8.ToArray();
    var stream = new MemoryStream();
    stream.Write(value);
    var contentType = MediaTypeNames.Text.Plain;
    var fileDownloadName = "foo.txt";
    var lastModified = DateTimeOffset.Now;
    var entityTag = new EntityTagHeaderValue("\"fooETag\"");
    ;
    var enableRangeProcessing = true;

    var result =
      (
        await Task.FromResult(Result.Success(stream))
          .ToFileStreamHttpResult(contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing)
      ).Result as FileStreamHttpResult;

    stream = new MemoryStream();
    result!.FileStream.Position = 0;
    await result!.FileStream.CopyToAsync(stream);
    stream.ToArray().Should().BeEquivalentTo(value);
    result!.LastModified.Should().Be(lastModified);
    result!.FileDownloadName.Should().Be(fileDownloadName);
    result!.FileLength.Should().Be(value.Length);
    result!.ContentType.Should().Be(contentType);
    result!.EnableRangeProcessing.Should().Be(enableRangeProcessing);
    result!.EntityTag.Should().Be(entityTag);
  }

  [Fact]
  public void ResultStream_Failure_can_be_mapped_to_FileStreamHttpResult()
  {
    var error = "Error";

    var result = Result.Failure<Stream>(error).ToFileStreamHttpResult().Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultStream_Failure_can_be_mapped_to_FileStreamHttpResult_Async()
  {
    var error = "Error";

    var result =
      (await Task.FromResult(Result.Failure<Stream>(error)).ToFileStreamHttpResult()).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultStream_Failure_StatusCode_can_be_changed()
  {
    var statusCode = 418;
    var error = "Error";

    var result =
      Result.Failure<Stream>(error).ToFileStreamHttpResult(failureStatusCode: statusCode).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ProblemDetails.Status.Should().Be(statusCode);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultStream_Failure_StatusCode_can_be_changed_Async()
  {
    var statusCode = 418;
    var error = "Error";

    var result =
      (
        await Task.FromResult(Result.Failure<Stream>(error)).ToFileStreamHttpResult(failureStatusCode: statusCode)
      ).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ProblemDetails.Status.Should().Be(statusCode);
    result!.ProblemDetails.Detail.Should().Be(error);
  }
}

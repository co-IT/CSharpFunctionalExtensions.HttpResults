using System.Net.Mime;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Net.Http.Headers;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToFileStreamHttpResultStreamE
{
    [Fact]
    public void ResultStreamE_Success_can_be_mapped_to_FileStreamHttpResult()
    {
        var value = "foo"u8.ToArray();
        var stream = new MemoryStream();
        stream.Write(value);
        var contentType = MediaTypeNames.Text.Plain;
        var fileDownloadName = "foo.txt";
        var lastModified = DateTimeOffset.Now;
        var entityTag = new EntityTagHeaderValue("\"fooETag\"");;
        var enableRangeProcessing = true;
        
        var result = Result.Success<Stream, DocumentMissingError>(stream)
            .ToFileStreamHttpResult(contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing).Result as FileStreamHttpResult;

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
    public async Task ResultStreamE_Success_can_be_mapped_to_FileStreamHttpResult_Async()
    {
        var value = "foo"u8.ToArray();
        var stream = new MemoryStream();
        stream.Write(value);
        var contentType = MediaTypeNames.Text.Plain;
        var fileDownloadName = "foo.txt";
        var lastModified = DateTimeOffset.Now;
        var entityTag = new EntityTagHeaderValue("\"fooETag\"");;
        var enableRangeProcessing = true;
        
        var result = (await Task.FromResult(Result.Success<Stream, DocumentMissingError>(stream))
            .ToFileStreamHttpResult(contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing)).Result as FileStreamHttpResult;
        
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
    public void ResultStreamE_Failure_can_be_mapped_to_FileStreamHttpResult()
    {
        var error = new DocumentMissingError
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = Result.Failure<Stream, DocumentMissingError>(error)
            .ToFileStreamHttpResult().Result as NotFound<string>;
        
        result!.StatusCode.Should().Be(404);
        result!.Value.Should().Be(error.DocumentId);
    }
    
    [Fact]
    public async Task ResultStreamE_Failure_can_be_mapped_to_FileStreamHttpResult_Async()
    {
        var error = new DocumentMissingError
        {
            DocumentId = Guid.NewGuid().ToString()
        };

        var result = (await Task.FromResult(Result.Failure<Stream, DocumentMissingError>(error))
            .ToFileStreamHttpResult()).Result as NotFound<string>;
        
        result!.StatusCode.Should().Be(404);
        result!.Value.Should().Be(error.DocumentId);
    }
}
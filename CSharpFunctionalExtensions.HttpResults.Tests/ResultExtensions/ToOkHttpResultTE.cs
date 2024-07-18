using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToOkHttpResultTE
{
    [Fact]
    public void ResultTE_Success_can_be_mapped_to_OkHttpResult()
    {
        var document = new Document
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = Result.Success<Document, DocumentMissingError>(document)
            .ToOkHttpResult().Result as Ok<Document>;
        
        result!.StatusCode.Should().Be(200);
        result!.Value!.DocumentId.Should().Be(document.DocumentId);
    }
    
    [Fact]
    public async Task ResultTE_Success_can_be_mapped_to_OkHttpResult_Async()
    {
        var document = new Document
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = (await Task.FromResult(Result.Success<Document, DocumentMissingError>(document))
            .ToOkHttpResult()).Result as Ok<Document>;
        
        result!.StatusCode.Should().Be(200);
        result!.Value!.DocumentId.Should().Be(document.DocumentId);
    }
    
    [Fact]
    public void ResultTE_Failure_can_be_mapped_to_OkHttpResult()
    {
        var error = new DocumentMissingError
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = Result.Failure<Document, DocumentMissingError>(error)
            .ToOkHttpResult().Result as NotFound<string>;
        
        result!.StatusCode.Should().Be(404);
        result!.Value.Should().Be(error.DocumentId);
    }
    
    [Fact]
    public async Task ResultTE_Failure_can_be_mapped_to_OkHttpResult_Async()
    {
        var error = new DocumentMissingError
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = (await Task.FromResult(Result.Failure<Document, DocumentMissingError>(error))
            .ToOkHttpResult()).Result as NotFound<string>;
        
        result!.StatusCode.Should().Be(404);
        result!.Value.Should().Be(error.DocumentId);
    }
}
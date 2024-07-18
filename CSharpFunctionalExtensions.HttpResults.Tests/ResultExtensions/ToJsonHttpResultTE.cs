using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToJsonHttpResultTE
{
    [Fact]
    public void ResultTE_Success_can_be_mapped_to_JsonHttpResult()
    {
        var document = new Document
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = Result.Success<Document, DocumentMissingError>(document)
            .ToJsonHttpResult().Result as JsonHttpResult<Document>;
        
        result!.StatusCode.Should().Be(200);
        result!.Value!.DocumentId.Should().Be(document.DocumentId);
    }
    
    [Fact]
    public async Task ResultTE_Success_can_be_mapped_to_JsonHttpResult_Async()
    {
        var document = new Document
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = (await Task.FromResult(Result.Success<Document, DocumentMissingError>(document))
            .ToJsonHttpResult()).Result as JsonHttpResult<Document>;
        
        result!.StatusCode.Should().Be(200);
        result!.Value!.DocumentId.Should().Be(document.DocumentId);
    }
    
    [Fact]
    public void ResultTE_Success_StatusCode_can_be_changed()
    {
        var statusCode = 210;
        var document = new Document
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = Result.Success<Document, DocumentMissingError>(document)
            .ToJsonHttpResult(statusCode).Result as JsonHttpResult<Document>;
        
        result!.StatusCode.Should().Be(statusCode);
    }
    
    [Fact]
    public async Task ResultTE_Success_StatusCode_can_be_changed_Async()
    {
        var statusCode = 210;
        var document = new Document
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = (await Task.FromResult(Result.Success<Document, DocumentMissingError>(document))
            .ToJsonHttpResult(statusCode)).Result as JsonHttpResult<Document>;
        
        result!.StatusCode.Should().Be(statusCode);
    }
    
    [Fact]
    public void ResultTE_Failure_can_be_mapped_to_JsonHttpResult()
    {
        var error = new DocumentMissingError
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = Result.Failure<Document, DocumentMissingError>(error)
            .ToJsonHttpResult().Result as NotFound<string>;
        
        result!.StatusCode.Should().Be(404);
        result!.Value.Should().Be(error.DocumentId);
    }
    
    [Fact]
    public async Task ResultTE_Failure_can_be_mapped_to_JsonHttpResult_Async()
    {
        var error = new DocumentMissingError
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = (await Task.FromResult(Result.Failure<Document, DocumentMissingError>(error))
            .ToJsonHttpResult()).Result as NotFound<string>;
        
        result!.StatusCode.Should().Be(404);
        result!.Value.Should().Be(error.DocumentId);
    }
}
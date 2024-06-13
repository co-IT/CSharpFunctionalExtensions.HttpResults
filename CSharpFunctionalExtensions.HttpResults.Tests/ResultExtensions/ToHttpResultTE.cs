using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToHttpResultTE
{
    [Fact]
    public void ResultTE_Success_can_be_mapped_to_HttpResult()
    {
        var document = new Document
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = Result.Success<Document, DocumentMissingError>(document)
            .ToHttpResult() as JsonHttpResult<Document>;
        
        result!.StatusCode.Should().Be(200);
        result!.Value.DocumentId.Should().Be(document.DocumentId);
    }
    
    [Fact]
    public async Task ResultTE_Success_can_be_mapped_to_HttpResult_Async()
    {
        var document = new Document
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = await Task.FromResult(Result.Success<Document, DocumentMissingError>(document))
            .ToHttpResult() as JsonHttpResult<Document>;
        
        result!.StatusCode.Should().Be(200);
        result!.Value.DocumentId.Should().Be(document.DocumentId);
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
            .ToHttpResult(statusCode) as JsonHttpResult<Document>;
        
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
        
        var result = await Task.FromResult(Result.Success<Document, DocumentMissingError>(document))
            .ToHttpResult(statusCode) as JsonHttpResult<Document>;
        
        result!.StatusCode.Should().Be(statusCode);
    }
    
    [Fact]
    public void ResultTE_Failure_can_be_mapped_to_HttpResult()
    {
        var error = new DocumentMissingError
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = Result.Failure<Document, DocumentMissingError>(error)
            .ToHttpResult() as NotFound<string>;
        
        result!.StatusCode.Should().Be(404);
        result!.Value.Should().Be(error.DocumentId);
    }
    
    [Fact]
    public async Task ResultTE_Failure_can_be_mapped_to_HttpResult_Async()
    {
        var error = new DocumentMissingError
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = await Task.FromResult(Result.Failure<Document, DocumentMissingError>(error))
            .ToHttpResult() as NotFound<string>;
        
        result!.StatusCode.Should().Be(404);
        result!.Value.Should().Be(error.DocumentId);
    }
}

public record Document
{
    public required string DocumentId { get; init; }
}

public class DocumentMissingError : IResultError
{
    public required string DocumentId { get; init; }
}

public class DocumentMissingErrorMapper : IResultErrorMapper<DocumentMissingError, Microsoft.AspNetCore.Http.IResult>
{
    public Func<DocumentMissingError, Microsoft.AspNetCore.Http.IResult> Map => error => TypedResults.NotFound(error.DocumentId);
}
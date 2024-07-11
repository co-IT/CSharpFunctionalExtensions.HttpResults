using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.UnitResultExtensions;

public class ToHttpResultE
{
    [Fact]
    public void UnitResultE_Success_can_be_mapped_to_HttpResult()
    {
        var result = UnitResult.Success<DocumentMissingError>()
            .ToHttpResult().Result as StatusCodeHttpResult;
        
        result!.StatusCode.Should().Be(204);
    }
    
    [Fact]
    public async Task UnitResultE_Success_can_be_mapped_to_HttpResult_Async()
    {
        var result = (await Task.FromResult(UnitResult.Success<DocumentMissingError>())
            .ToHttpResult()).Result as StatusCodeHttpResult;
        
        result!.StatusCode.Should().Be(204);
    }
    
    [Fact]
    public void UnitResultE_Success_StatusCode_can_be_changed()
    {
        var statusCode = 210;
        
        var result = UnitResult.Success<DocumentMissingError>()
            .ToHttpResult(statusCode).Result as StatusCodeHttpResult;
        
        result!.StatusCode.Should().Be(statusCode);
    }
    
    [Fact]
    public async Task UnitResultE_Success_StatusCode_can_be_changed_Async()
    {
        var statusCode = 210;
        
        var result = (await Task.FromResult(UnitResult.Success<DocumentMissingError>())
            .ToHttpResult(statusCode)).Result as StatusCodeHttpResult;
        
        result!.StatusCode.Should().Be(statusCode);
    }
    
    [Fact]
    public void UnitResultE_Failure_can_be_mapped_to_HttpResult()
    {
        var error = new DocumentMissingError
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = UnitResult.Failure(error)
            .ToHttpResult().Result as NotFound<string>;
        
        result!.StatusCode.Should().Be(404);
        result!.Value.Should().Be(error.DocumentId);
    }
    
    [Fact]
    public async Task UnitResultE_Failure_can_be_mapped_to_HttpResult_Async()
    {
        var error = new DocumentMissingError
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        
        var result = (await Task.FromResult(UnitResult.Failure(error))
            .ToHttpResult()).Result as NotFound<string>;
        
        result!.StatusCode.Should().Be(404);
        result!.Value.Should().Be(error.DocumentId);
    }
}
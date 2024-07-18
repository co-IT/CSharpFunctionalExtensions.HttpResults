using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToNoContentHttpResultT
{
    [Fact]
    public void ResultT_Success_can_be_mapped_to_NoContentHttpResult()
    {
        var value = "foo";
        
        var result = Result.Success(value)
            .ToNoContentHttpResult().Result as NoContent;
        
        result!.StatusCode.Should().Be(204);
    }
    
    [Fact]
    public async Task ResultT_Success_can_be_mapped_to_NoContentHttpResult_Async()
    {
        var value = "foo";
        
        var result = (await Task.FromResult(Result.Success(value))
            .ToNoContentHttpResult()).Result as NoContent;
        
        result!.StatusCode.Should().Be(204);
    }
    
    [Fact]
    public void ResultT_Failure_can_be_mapped_to_HttpResult()
    {
        var error = "Error";
        
        var result = Result.Failure<string>(error)
            .ToNoContentHttpResult().Result as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(400);
        result!.ProblemDetails.Status.Should().Be(400);
        result!.ProblemDetails.Detail.Should().Be(error);
    }
    
    [Fact]
    public async Task ResultT_Failure_can_be_mapped_to_NoContentHttpResult_Async()
    {
        var error = "Error";

        var result = (await Task.FromResult(Result.Failure<string>(error))
            .ToNoContentHttpResult()).Result as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(400);
        result!.ProblemDetails.Status.Should().Be(400);
        result!.ProblemDetails.Detail.Should().Be(error);
    }
    
    [Fact]
    public void ResultT_Failure_StatusCode_can_be_changed()
    {
        var statusCode = 418;
        var error = "Error";
        
        var result = Result.Failure<string>(error)
            .ToNoContentHttpResult(failureStatusCode: statusCode).Result as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(statusCode);
        result!.ProblemDetails.Status.Should().Be(statusCode);
        result!.ProblemDetails.Detail.Should().Be(error);
    }
    
    [Fact]
    public async Task ResultT_Failure_StatusCode_can_be_changed_Async()
    {
        var statusCode = 418;
        var error = "Error";
        
        var result = (await Task.FromResult(Result.Failure<string>(error))
            .ToNoContentHttpResult(failureStatusCode: statusCode)).Result as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(statusCode);
        result!.ProblemDetails.Status.Should().Be(statusCode);
        result!.ProblemDetails.Detail.Should().Be(error);
    }
}
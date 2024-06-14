using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToHttpResult
{
    [Fact]
    public void Result_Success_can_be_mapped_to_HttpResult()
    {
        var result = Result.Success()
            .ToHttpResult() as StatusCodeHttpResult;
        
        result!.StatusCode.Should().Be(204);
    }
    
    [Fact]
    public async Task Result_Success_can_be_mapped_to_HttpResult_Async()
    {
        var result = await Task.FromResult(Result.Success())
            .ToHttpResult() as StatusCodeHttpResult;
        
        result!.StatusCode.Should().Be(204);
    }
    
    [Fact]
    public void Result_Success_StatusCode_can_be_changed()
    {
        var statusCode = 210;
        
        var result = Result.Success()
            .ToHttpResult(statusCode) as StatusCodeHttpResult;
        
        result!.StatusCode.Should().Be(statusCode);
    }
    
    [Fact]
    public async Task Result_Success_StatusCode_can_be_changed_Async()
    {
        var statusCode = 210;
        
        var result = await Task.FromResult(Result.Success())
            .ToHttpResult(statusCode) as StatusCodeHttpResult;
        
        result!.StatusCode.Should().Be(statusCode);
    }
    
    [Fact]
    public void Result_Failure_can_be_mapped_to_HttpResult()
    {
        var error = "Error";
        
        var result = Result.Failure(error)
            .ToHttpResult() as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(400);
        result!.ProblemDetails.Status.Should().Be(400);
        result!.ProblemDetails.Detail.Should().Be(error);
    }
    
    [Fact]
    public async Task Result_Failure_can_be_mapped_to_HttpResult_Async()
    {
        var error = "Error";
        
        var result = await Task.FromResult(Result.Failure(error))
            .ToHttpResult() as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(400);
        result!.ProblemDetails.Status.Should().Be(400);
        result!.ProblemDetails.Detail.Should().Be(error);
    }
    
    [Fact]
    public void Result_Failure_StatusCode_can_be_changed()
    {
        var statusCode = 418;
        
        var result = Result.FailureIf(true, "Error")
            .ToHttpResult(failureStatusCode: statusCode) as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(statusCode);
        result!.ProblemDetails.Status.Should().Be(statusCode);
    }
    
    [Fact]
    public async Task Result_Failure_StatusCode_can_be_changed_Async()
    {
        var statusCode = 418;
        
        var result = await Task.FromResult(Result.FailureIf(true, "Error"))
            .ToHttpResult(failureStatusCode: statusCode) as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(statusCode);
        result!.ProblemDetails.Status.Should().Be(statusCode);
    }
}
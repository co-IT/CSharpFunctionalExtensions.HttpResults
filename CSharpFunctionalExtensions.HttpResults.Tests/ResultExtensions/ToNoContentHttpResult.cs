using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToNoContentHttpResult
{
    [Fact]
    public void Result_Success_can_be_mapped_to_NoContentHttpResult()
    {
        var result = Result.Success()
            .ToNoContentHttpResult().Result as NoContent;
        
        result!.StatusCode.Should().Be(204);
    }
    
    [Fact]
    public async Task Result_Success_can_be_mapped_to_NoContentHttpResult_Async()
    {
        var result = (await Task.FromResult(Result.Success())
            .ToNoContentHttpResult()).Result as NoContent;
        
        result!.StatusCode.Should().Be(204);
    }
    
    [Fact]
    public void Result_Failure_can_be_mapped_to_NoContentHttpResult()
    {
        var error = "Error";
        
        var result = Result.Failure(error)
            .ToNoContentHttpResult().Result as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(400);
        result!.ProblemDetails.Status.Should().Be(400);
        result!.ProblemDetails.Detail.Should().Be(error);
    }
    
    [Fact]
    public async Task Result_Failure_can_be_mapped_to_NoContentHttpResult_Async()
    {
        var error = "Error";

        var result = (await Task.FromResult(Result.Failure(error))
            .ToNoContentHttpResult()).Result as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(400);
        result!.ProblemDetails.Status.Should().Be(400);
        result!.ProblemDetails.Detail.Should().Be(error);
    }
    
    [Fact]
    public void Result_Failure_StatusCode_can_be_changed()
    {
        var statusCode = 418;
        var error = "Error";
        
        var result = Result.Failure(error)
            .ToNoContentHttpResult(failureStatusCode: statusCode).Result as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(statusCode);
        result!.ProblemDetails.Status.Should().Be(statusCode);
        result!.ProblemDetails.Detail.Should().Be(error);
    }
    
    [Fact]
    public async Task Result_Failure_StatusCode_can_be_changed_Async()
    {
        var statusCode = 418;
        var error = "Error";
        
        var result = (await Task.FromResult(Result.Failure(error))
            .ToNoContentHttpResult(failureStatusCode: statusCode)).Result as ProblemHttpResult;
        
        result!.StatusCode.Should().Be(statusCode);
        result!.ProblemDetails.Status.Should().Be(statusCode);
        result!.ProblemDetails.Detail.Should().Be(error);
    }
}
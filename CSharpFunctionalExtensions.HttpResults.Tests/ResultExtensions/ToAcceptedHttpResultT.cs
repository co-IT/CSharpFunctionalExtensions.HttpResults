using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToAcceptedHttpResultT
{
    [Fact]
    public void ResultT_Success_can_be_mapped_to_AcceptedHttpResult()
    {
        var document = new Document
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        Func<Document, Uri > uri = x => new Uri($"http://localhost/documents/{x.DocumentId}");

        var result = Result.Success(document)
            .ToAcceptedHttpResult(
                uri
            ).Result as Accepted<Document>;

        result!.StatusCode.Should().Be(202);
        result!.Value.Should().Be(document);
        result!.Location.Should().Be(uri(document).AbsoluteUri);
    }

    [Fact]
    public async Task ResultT_Success_can_be_mapped_to_AcceptedHttpResult_Async()
    {
        var document = new Document
        {
            DocumentId = Guid.NewGuid().ToString()
        };
        Func<Document, Uri > uri = x => new Uri($"http://localhost/documents/{x.DocumentId}");

        var result = (await Task.FromResult(Result.Success(document))
            .ToAcceptedHttpResult(
                uri
            )).Result as Accepted<Document>;

        result!.StatusCode.Should().Be(202);
        result!.Value.Should().Be(document);
        result!.Location.Should().Be(uri(document).AbsoluteUri);
    }

    [Fact]
    public void ResultT_Failure_can_be_mapped_to_AcceptedHttpResult()
    {
        var error = "Error";
        
        Func<Document, Uri > uri = x => new Uri($"http://localhost/documents/{x.DocumentId}");

        var result = Result.Failure<Document>(error)
            .ToAcceptedHttpResult(
                uri
            ).Result as ProblemHttpResult;

        result!.StatusCode.Should().Be(400);
        result!.ProblemDetails.Status.Should().Be(400);
        result!.ProblemDetails.Detail.Should().Be(error);
    }

    [Fact]
    public async Task ResultT_Failure_can_be_mapped_to_AcceptedHttpResult_Async()
    {
        var error = "Error";
        
        Func<Document, Uri > uri = x => new Uri($"http://localhost/documents/{x.DocumentId}");

        var result = (await Task.FromResult(Result.Failure<Document>(error))
            .ToAcceptedHttpResult(
                uri
            )).Result as ProblemHttpResult;

        result!.StatusCode.Should().Be(400);
        result!.ProblemDetails.Status.Should().Be(400);
        result!.ProblemDetails.Detail.Should().Be(error);
    }

    [Fact]
    public void ResultT_Failure_StatusCode_can_be_changed()
    {
        var statusCode = 418;
        var error = "Error";
        
        Func<Document, Uri > uri = x => new Uri($"http://localhost/documents/{x.DocumentId}");

        var result = Result.Failure<Document>(error)
            .ToAcceptedHttpResult(
                uri,
                failureStatusCode: statusCode
            ).Result as ProblemHttpResult;

        result!.StatusCode.Should().Be(statusCode);
        result!.ProblemDetails.Status.Should().Be(statusCode);
        result!.ProblemDetails.Detail.Should().Be(error);
    }

    [Fact]
    public async Task ResultT_Failure_StatusCode_can_be_changed_Async()
    {
        var statusCode = 418;
        var error = "Error";
        
        Func<Document, Uri > uri = x => new Uri($"http://localhost/documents/{x.DocumentId}");

        var result = (await Task.FromResult(Result.Failure<Document>(error))
            .ToAcceptedHttpResult(
                uri,
                failureStatusCode: statusCode
            )).Result as ProblemHttpResult;

        result!.StatusCode.Should().Be(statusCode);
        result!.ProblemDetails.Status.Should().Be(statusCode);
        result!.ProblemDetails.Detail.Should().Be(error);
    }
}
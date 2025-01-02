using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToAcceptedAtRouteHttpResultT
{
  [Fact]
  public void ResultT_Success_can_be_mapped_to_AcceptedAtRouteHttpResult()
  {
    var document = new Document { DocumentId = Guid.NewGuid().ToString() };
    var routeName = "documents";
    var routeValueKey = "id";

    var result =
      Result
        .Success(document)
        .ToAcceptedAtRouteHttpResult(routeName, x => new Dictionary<string, string> { { routeValueKey, x.DocumentId } })
        .Result as AcceptedAtRoute<Document>;

    result!.StatusCode.Should().Be(202);
    result!.Value.Should().Be(document);
    result!.RouteName.Should().Be(routeName);
    result!.RouteValues[routeValueKey].Should().Be(document.DocumentId);
  }

  [Fact]
  public async Task ResultT_Success_can_be_mapped_to_AcceptedAtRouteHttpResult_Async()
  {
    var document = new Document { DocumentId = Guid.NewGuid().ToString() };
    var routeName = "documents";
    var routeValueKey = "id";

    var result =
      (
        await Task.FromResult(Result.Success(document))
          .ToAcceptedAtRouteHttpResult(
            routeName,
            x => new Dictionary<string, string> { { routeValueKey, x.DocumentId } }
          )
      ).Result as AcceptedAtRoute<Document>;

    result!.StatusCode.Should().Be(202);
    result!.Value.Should().Be(document);
    result!.RouteName.Should().Be(routeName);
    result!.RouteValues[routeValueKey].Should().Be(document.DocumentId);
  }

  [Fact]
  public void ResultT_Failure_can_be_mapped_to_AcceptedAtRouteHttpResult()
  {
    var error = "Error";

    var result = Result.Failure<Document>(error).ToAcceptedAtRouteHttpResult().Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultT_Failure_can_be_mapped_to_AcceptedAtRouteHttpResult_Async()
  {
    var error = "Error";

    var result =
      (await Task.FromResult(Result.Failure<Document>(error)).ToAcceptedAtRouteHttpResult()).Result
      as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result!.ProblemDetails.Status.Should().Be(400);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultT_Failure_StatusCode_can_be_changed()
  {
    var statusCode = 418;
    var error = "Error";

    var result =
      Result.Failure<Document>(error).ToAcceptedAtRouteHttpResult(failureStatusCode: statusCode).Result
      as ProblemHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ProblemDetails.Status.Should().Be(statusCode);
    result!.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultT_Failure_StatusCode_can_be_changed_Async()
  {
    var statusCode = 418;
    var error = "Error";

    var result =
      (
        await Task.FromResult(Result.Failure<Document>(error))
          .ToAcceptedAtRouteHttpResult(failureStatusCode: statusCode)
      ).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(statusCode);
    result!.ProblemDetails.Status.Should().Be(statusCode);
    result!.ProblemDetails.Detail.Should().Be(error);
  }
}

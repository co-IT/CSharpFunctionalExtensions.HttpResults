using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToCreatedAtRouteHttpResultT
{
  [Fact]
  public void ResultT_Success_can_be_mapped_to_CreatedAtRouteHttpResult()
  {
    var document = new Document { DocumentId = Guid.NewGuid().ToString() };
    var routeName = "documents";
    var routeValueKey = "id";

    var result = Assert.IsType<CreatedAtRoute<Document>>(
      Result
        .Success(document)
        .ToCreatedAtRouteHttpResult(routeName, x => new Dictionary<string, string> { { routeValueKey, x.DocumentId } })
        .Result
    );

    result.StatusCode.Should().Be(201);
    result.Value.Should().Be(document);
    result.RouteName.Should().Be(routeName);
    result.RouteValues[routeValueKey].Should().Be(document.DocumentId);
  }

  [Fact]
  public async Task ResultT_Success_can_be_mapped_to_CreatedAtRouteHttpResult_Async()
  {
    var document = new Document { DocumentId = Guid.NewGuid().ToString() };
    var routeName = "documents";
    var routeValueKey = "id";

    var result = Assert.IsType<CreatedAtRoute<Document>>(
      (
        await Task.FromResult(Result.Success(document))
          .ToCreatedAtRouteHttpResult(
            routeName,
            x => new Dictionary<string, string> { { routeValueKey, x.DocumentId } }
          )
      ).Result
    );

    result.StatusCode.Should().Be(201);
    result.Value.Should().Be(document);
    result.RouteName.Should().Be(routeName);
    result.RouteValues[routeValueKey].Should().Be(document.DocumentId);
  }

  [Fact]
  public void ResultT_Failure_can_be_mapped_to_CreatedAtRouteHttpResult()
  {
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(Result.Failure<Document>(error).ToCreatedAtRouteHttpResult().Result);

    result.StatusCode.Should().Be(400);
    result.ProblemDetails.Status.Should().Be(400);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultT_Failure_can_be_mapped_to_CreatedAtRouteHttpResult_Async()
  {
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      (await Task.FromResult(Result.Failure<Document>(error)).ToCreatedAtRouteHttpResult()).Result
    );

    result.StatusCode.Should().Be(400);
    result.ProblemDetails.Status.Should().Be(400);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultT_Failure_StatusCode_can_be_changed()
  {
    var statusCode = 418;
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      Result.Failure<Document>(error).ToCreatedAtRouteHttpResult(failureStatusCode: statusCode).Result
    );

    result.StatusCode.Should().Be(statusCode);
    result.ProblemDetails.Status.Should().Be(statusCode);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultT_Failure_StatusCode_can_be_changed_Async()
  {
    var statusCode = 418;
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      (
        await Task.FromResult(Result.Failure<Document>(error)).ToCreatedAtRouteHttpResult(failureStatusCode: statusCode)
      ).Result
    );

    result.StatusCode.Should().Be(statusCode);
    result.ProblemDetails.Status.Should().Be(statusCode);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultT_Failure_ProblemDetails_can_be_customized()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result = Assert.IsType<ProblemHttpResult>(
      Result
        .Failure<Document>(error)
        .ToCreatedAtRouteHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
        .Result
    );

    result.ProblemDetails.Title.Should().Be(customTitle);
  }

  [Fact]
  public async Task ResultT_Failure_ProblemDetails_can_be_customized_Async()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result = Assert.IsType<ProblemHttpResult>(
      (
        await Task.FromResult(Result.Failure<Document>(error))
          .ToCreatedAtRouteHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
      ).Result
    );

    result.ProblemDetails.Title.Should().Be(customTitle);
  }
}

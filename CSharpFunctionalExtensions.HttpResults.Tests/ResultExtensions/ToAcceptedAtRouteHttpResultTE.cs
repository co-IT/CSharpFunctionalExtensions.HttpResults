using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToAcceptedAtRouteHttpResultTE
{
  [Fact]
  public void ResultTE_Success_can_be_mapped_to_AcceptedAtRouteHttpResult()
  {
    var document = new Document { DocumentId = Guid.NewGuid().ToString() };
    var routeName = "documents";
    var routeValueKey = "id";

    var result = Assert.IsType<AcceptedAtRoute<Document>>(
      Result
        .Success<Document, DocumentCreationError>(document)
        .ToAcceptedAtRouteHttpResult(routeName, x => new Dictionary<string, string> { { routeValueKey, x.DocumentId } })
        .Result
    );

    result.StatusCode.Should().Be(202);
    result.Value.Should().Be(document);
    result.RouteName.Should().Be(routeName);
    result.RouteValues[routeValueKey].Should().Be(document.DocumentId);
  }

  [Fact]
  public async Task ResultTE_Success_can_be_mapped_to_AcceptedAtRouteHttpResult_Async()
  {
    var document = new Document { DocumentId = Guid.NewGuid().ToString() };
    var routeName = "documents";
    var routeValueKey = "id";

    var result = Assert.IsType<AcceptedAtRoute<Document>>(
      (
        await Task.FromResult(Result.Success<Document, DocumentCreationError>(document))
          .ToAcceptedAtRouteHttpResult(
            routeName,
            x => new Dictionary<string, string> { { routeValueKey, x.DocumentId } }
          )
      ).Result
    );

    result.StatusCode.Should().Be(202);
    result.Value.Should().Be(document);
    result.RouteName.Should().Be(routeName);
    result.RouteValues[routeValueKey].Should().Be(document.DocumentId);
  }

  [Fact]
  public void ResultTE_Failure_can_be_mapped_to_AcceptedAtRouteHttpResult()
  {
    var error = new DocumentCreationError { DocumentId = Guid.NewGuid().ToString() };

    var result = Assert.IsType<Conflict<string>>(
      Result.Failure<Document, DocumentCreationError>(error).ToAcceptedAtRouteHttpResult().Result
    );

    result.StatusCode.Should().Be(409);
    result.Value.Should().Be(error.DocumentId);
  }

  [Fact]
  public async Task ResultTE_Failure_can_be_mapped_to_AcceptedAtRouteHttpResult_Async()
  {
    var error = new DocumentCreationError { DocumentId = Guid.NewGuid().ToString() };

    var result = Assert.IsType<Conflict<string>>(
      (
        await Task.FromResult(Result.Failure<Document, DocumentCreationError>(error)).ToAcceptedAtRouteHttpResult()
      ).Result
    );

    result.StatusCode.Should().Be(409);
    result.Value.Should().Be(error.DocumentId);
  }
}

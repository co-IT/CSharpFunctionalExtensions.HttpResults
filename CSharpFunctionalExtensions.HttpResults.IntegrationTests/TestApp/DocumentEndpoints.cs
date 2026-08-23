using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CSharpFunctionalExtensions.HttpResults.IntegrationTests.TestApp;

public static class DocumentEndpoints
{
  public static IEndpointRouteBuilder MapDocumentEndpoints(this IEndpointRouteBuilder endpoints)
  {
    endpoints.MapGet(
      "/documents/{id}",
      (string id, HttpContext httpContext) =>
        Result.Failure<string, DocumentNotFoundError>(new DocumentNotFoundError(id)).ToOkHttpResult(httpContext)
    );

    return endpoints;
  }
}

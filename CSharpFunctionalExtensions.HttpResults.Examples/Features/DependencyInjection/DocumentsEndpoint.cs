using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.DependencyInjection;

public static class DocumentsEndpoint
{
  public static IEndpointRouteBuilder MapDocuments(this IEndpointRouteBuilder endpoints)
  {
    endpoints.MapGet("/documents/{id}", Handle).WithName(nameof(DocumentsEndpoint));

    return endpoints;
  }

  private static Results<Ok<string>, ProblemHttpResult> Handle(string id, HttpContext httpContext)
  {
    return Result.Failure<string, DocumentNotFoundError>(new DocumentNotFoundError(id)).ToOkHttpResult(httpContext);
  }
}

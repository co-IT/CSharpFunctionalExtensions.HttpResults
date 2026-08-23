using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CSharpFunctionalExtensions.HttpResults.IntegrationTests.TestApp;

public static class BookEndpoints
{
  public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder endpoints)
  {
    endpoints.MapGet(
      "/books/{id}",
      (string id, HttpContext httpContext) =>
        Result
          .Failure<int>($"Book {id} not found")
          .ToOkHttpResult(
            httpContext,
            failureStatusCode: 404,
            customizeProblemDetails: problemDetails => problemDetails.Extensions["callback"] = "ran-last"
          )
    );

    endpoints.MapGet(
      "/static/books/{id}",
      (string id) => Result.Failure<int>($"Book {id} not found").ToOkHttpResult(failureStatusCode: 404)
    );

    endpoints.MapGet("/books", (HttpContext httpContext) => Result.Success("all-books").ToOkHttpResult(httpContext));

    return endpoints;
  }
}

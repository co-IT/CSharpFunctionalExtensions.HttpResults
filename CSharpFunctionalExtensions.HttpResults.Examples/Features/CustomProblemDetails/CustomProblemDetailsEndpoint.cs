using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CustomProblemDetails;

/// <summary>
///   The generator discovers and registers the factory-backed IResultProblemDetailsProvider. Its presence
///   enables the generated context overloads for built-in string-error results.
/// </summary>
public static class CustomProblemDetailsEndpoint
{
  public static IEndpointRouteBuilder MapCustomProblemDetails(this IEndpointRouteBuilder endpoints)
  {
    endpoints.MapGet("/custom-problem-details", Handle).WithName(nameof(CustomProblemDetailsEndpoint));

    return endpoints;
  }

  private static Results<Ok<int>, ProblemHttpResult> Handle(HttpContext httpContext)
  {
    return Result
      .Failure<int>("Example failure")
      .ToOkHttpResult(
        httpContext,
        failureStatusCode: 400,
        customizeProblemDetails: problemDetails =>
          problemDetails.Extensions["source"] = "custom-problem-details-example"
      );
  }
}

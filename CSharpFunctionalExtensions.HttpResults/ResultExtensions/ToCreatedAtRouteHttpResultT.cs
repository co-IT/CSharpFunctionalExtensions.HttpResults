using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.ResultExtensions;

/// <summary>
///   Extension methods for <see cref="Result{T}" />
/// </summary>
public static partial class ResultExtensions
{
  /// <summary>
  ///   Returns a <see cref="CreatedAtRoute{TValue}" /> with Created status code in case of success result. Returns
  ///   <see cref="ProblemHttpResult" /> in case of failure. You can provide route info to create a location HTTP-Header. You
  ///   can override the error status code.
  /// </summary>
  public static Results<CreatedAtRoute<T>, ProblemHttpResult> ToCreatedAtRouteHttpResult<T>(
    this Result<T> result,
    string? routeName = null,
    Func<T, object>? routeValues = null,
    int failureStatusCode = 400,
    Action<ProblemDetails>? customizeProblemDetails = null
  )
  {
    if (result.IsSuccess)
      return TypedResults.CreatedAtRoute(result.Value, routeName, routeValues?.Invoke(result.Value));

    var problemDetailsInfo = ProblemDetailsMappingProvider.FindMapping(failureStatusCode);
    var problemDetails = new ProblemDetails
    {
      Status = failureStatusCode,
      Title = problemDetailsInfo.Title,
      Type = problemDetailsInfo.Type,
      Detail = result.Error,
    };

    customizeProblemDetails?.Invoke(problemDetails);

    return TypedResults.Problem(problemDetails);
  }

  /// <summary>
  ///   Returns a <see cref="CreatedAtRoute{TValue}" /> with Created status code in case of success result. Returns
  ///   <see cref="ProblemHttpResult" /> in case of failure. You can provide route info to create a location HTTP-Header. You
  ///   can override the error status code.
  /// </summary>
  public static async Task<Results<CreatedAtRoute<T>, ProblemHttpResult>> ToCreatedAtRouteHttpResult<T>(
    this Task<Result<T>> result,
    string? routeName = null,
    Func<T, object>? routeValues = null,
    int failureStatusCode = 400,
    Action<ProblemDetails>? customizeProblemDetails = null
  )
  {
    return (await result).ToCreatedAtRouteHttpResult(
      routeName,
      routeValues,
      failureStatusCode,
      customizeProblemDetails
    );
  }
}

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
  ///   Returns a <see cref="JsonHttpResult{TValue}" /> in case of success result. Returns <see cref="ProblemHttpResult" />
  ///   in case of failure. You can override the success and error status code.
  /// </summary>
  public static Results<JsonHttpResult<T>, ProblemHttpResult> ToJsonHttpResult<T>(
    this Result<T> result,
    int successStatusCode = 200,
    int failureStatusCode = 400,
    Action<ProblemDetails>? customizeProblemDetails = null
  )
  {
    if (result.IsSuccess)
      return TypedResults.Json(result.Value, statusCode: successStatusCode);

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
  ///   Returns a <see cref="JsonHttpResult{TValue}" /> in case of success result. Returns <see cref="ProblemHttpResult" />
  ///   in case of failure. You can override the success and error status code.
  /// </summary>
  public static async Task<Results<JsonHttpResult<T>, ProblemHttpResult>> ToJsonHttpResult<T>(
    this Task<Result<T>> result,
    int successStatusCode = 200,
    int failureStatusCode = 400,
    Action<ProblemDetails>? customizeProblemDetails = null
  )
  {
    return (await result).ToJsonHttpResult(successStatusCode, failureStatusCode, customizeProblemDetails);
  }
}

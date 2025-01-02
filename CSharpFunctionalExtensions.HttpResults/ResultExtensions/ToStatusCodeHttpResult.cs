using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.ResultExtensions;

/// <summary>
///   Extension methods for <see cref="Result" />
/// </summary>
public static partial class ResultExtensions
{
  /// <summary>
  ///   Returns a <see cref="StatusCodeHttpResult" /> in case of success result. Returns <see cref="ProblemHttpResult" /> in
  ///   case of failure. You can override the success and error status code.
  /// </summary>
  public static Results<StatusCodeHttpResult, ProblemHttpResult> ToStatusCodeHttpResult(
    this Result result,
    int successStatusCode = 204,
    int failureStatusCode = 400
  )
  {
    if (result.IsSuccess)
      return TypedResults.StatusCode(successStatusCode);

    var problemDetailsInfo = ProblemDetailsMap.Find(failureStatusCode);
    var problemDetails = new ProblemDetails
    {
      Status = failureStatusCode,
      Title = problemDetailsInfo.Title,
      Type = problemDetailsInfo.Type,
      Detail = result.Error,
    };

    return TypedResults.Problem(problemDetails);
  }

  /// <summary>
  ///   Returns a <see cref="StatusCodeHttpResult" /> in case of success result. Returns <see cref="ProblemHttpResult" /> in
  ///   case of failure. You can override the success and error status code.
  /// </summary>
  public static async Task<Results<StatusCodeHttpResult, ProblemHttpResult>> ToStatusCodeHttpResult(
    this Task<Result> result,
    int successStatusCode = 204,
    int failureStatusCode = 400
  )
  {
    return (await result).ToStatusCodeHttpResult(successStatusCode, failureStatusCode);
  }
}

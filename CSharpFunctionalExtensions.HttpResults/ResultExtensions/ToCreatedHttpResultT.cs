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
  ///   Returns a <see cref="Created{TValue}" /> with Created status code in case of success result. Returns
  ///   <see cref="ProblemHttpResult" /> in case of failure. You can provide an URI to create a location HTTP-Header. You can
  ///   override the error status code.
  /// </summary>
  public static Results<Created<T>, ProblemHttpResult> ToCreatedHttpResult<T>(
    this Result<T> result,
    Func<T, Uri>? uri = null,
    int failureStatusCode = 400
  )
  {
    if (result.IsSuccess)
      return uri is null
        ? TypedResults.Created(string.Empty, result.Value)
        : TypedResults.Created(uri.Invoke(result.Value), result.Value);

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
  ///   Returns a <see cref="Created{TValue}" /> with Created status code in case of success result. Returns
  ///   <see cref="ProblemHttpResult" /> in case of failure. You can provide an URI to create a location HTTP-Header. You can
  ///   override the error status code.
  /// </summary>
  public static async Task<Results<Created<T>, ProblemHttpResult>> ToCreatedHttpResult<T>(
    this Task<Result<T>> result,
    Func<T, Uri>? uri = null,
    int failureStatusCode = 400
  )
  {
    return (await result).ToCreatedHttpResult(uri, failureStatusCode);
  }
}

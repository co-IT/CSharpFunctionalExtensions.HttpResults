﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.ResultExtensions;

/// <summary>
///   Extension methods for <see cref="Result{T}" />
/// </summary>
public static partial class ResultExtensions
{
  /// <summary>
  ///   Returns a <see cref="NoContent" /> in case of success result. Returns <see cref="ProblemHttpResult" /> in case of
  ///   failure. You can override the error status code.
  /// </summary>
  public static Results<NoContent, ProblemHttpResult> ToNoContentHttpResult(
    this Result result,
    int failureStatusCode = 400,
    Action<ProblemDetails>? customizeProblemDetails = null
  )
  {
    if (result.IsSuccess)
      return TypedResults.NoContent();

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
  ///   Returns a <see cref="NoContent" /> in case of success result. Returns <see cref="ProblemHttpResult" /> in case of
  ///   failure. You can override the error status code.
  /// </summary>
  public static async Task<Results<NoContent, ProblemHttpResult>> ToNoContentHttpResult(
    this Task<Result> result,
    int failureStatusCode = 400,
    Action<ProblemDetails>? customizeProblemDetails = null
  )
  {
    return (await result).ToNoContentHttpResult(failureStatusCode, customizeProblemDetails);
  }
}

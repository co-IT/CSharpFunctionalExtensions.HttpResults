﻿using System.Text;
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
  ///   Returns a <see cref="ContentHttpResult" /> in case of success result. Returns <see cref="ProblemHttpResult" /> in case of
  ///   failure. You can override the error status code.
  /// </summary>
  public static Results<ContentHttpResult, ProblemHttpResult> ToContentHttpResult(
    this Result<string> result,
    string? contentType = null,
    Encoding? contentEncoding = null,
    int? statusCode = null,
    int failureStatusCode = 400,
    Action<ProblemDetails>? customizeProblemDetails = null
  )
  {
    if (result.IsSuccess)
      return TypedResults.Content(result.Value, contentType, contentEncoding, statusCode);

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
  ///   Returns a <see cref="ContentHttpResult" /> in case of success result. Returns <see cref="ProblemHttpResult" /> in case of
  ///   failure. You can override the error status code.
  /// </summary>
  public static async Task<Results<ContentHttpResult, ProblemHttpResult>> ToContentHttpResult(
    this Task<Result<string>> result,
    string? contentType = null,
    Encoding? contentEncoding = null,
    int? statusCode = null,
    int failureStatusCode = 400,
    Action<ProblemDetails>? customizeProblemDetails = null
  )
  {
    return (await result).ToContentHttpResult(
      contentType,
      contentEncoding,
      statusCode,
      failureStatusCode,
      customizeProblemDetails
    );
  }
}

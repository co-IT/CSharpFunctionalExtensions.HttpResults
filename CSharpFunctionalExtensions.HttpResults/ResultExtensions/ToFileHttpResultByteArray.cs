using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace CSharpFunctionalExtensions.HttpResults.ResultExtensions;

/// <summary>
///   Extension methods for <see cref="Result{T}" />
/// </summary>
public static partial class ResultExtensions
{
  /// <summary>
  ///   Returns a <see cref="FileContentHttpResult" /> based of a byte array in case of success result. Returns
  ///   <see cref="ProblemHttpResult" /> in case of failure. You can override the error status code.
  /// </summary>
  public static Results<FileContentHttpResult, ProblemHttpResult> ToFileHttpResult(
    this Result<byte[]> result,
    string? contentType = null,
    string? fileDownloadName = null,
    DateTimeOffset? lastModified = null,
    EntityTagHeaderValue? entityTag = null,
    bool enableRangeProcessing = false,
    int failureStatusCode = 400
  )
  {
    if (result.IsSuccess)
      return TypedResults.File(
        result.Value,
        contentType,
        fileDownloadName,
        enableRangeProcessing,
        lastModified,
        entityTag
      );

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
  ///   Returns a <see cref="FileContentHttpResult" /> based of a byte array in case of success result. Returns
  ///   <see cref="ProblemHttpResult" /> in case of failure. You can override the error status code.
  /// </summary>
  public static async Task<Results<FileContentHttpResult, ProblemHttpResult>> ToFileHttpResult(
    this Task<Result<byte[]>> result,
    string? contentType = null,
    string? fileDownloadName = null,
    DateTimeOffset? lastModified = null,
    EntityTagHeaderValue? entityTag = null,
    bool enableRangeProcessing = false,
    int failureStatusCode = 400
  )
  {
    return (await result).ToFileHttpResult(
      contentType,
      fileDownloadName,
      lastModified,
      entityTag,
      enableRangeProcessing,
      failureStatusCode
    );
  }
}

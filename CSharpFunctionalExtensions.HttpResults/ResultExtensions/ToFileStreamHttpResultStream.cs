using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace CSharpFunctionalExtensions.HttpResults.ResultExtensions;

/// <summary>
/// Extension methods for <see cref="Result{T}"/>
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Returns a <see cref="FileStreamHttpResult"/> based of a Stream in case of success result. Returns <see cref="ProblemHttpResult"/> in case of failure. You can override the error status code.
    /// </summary>
    public static Results<FileStreamHttpResult, ProblemHttpResult> ToFileStreamHttpResult<T>(this Result<T> result, string? contentType = null,
        string? fileDownloadName = null, DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null,
        bool enableRangeProcessing = false, int failureStatusCode = 400) where T : Stream
    {
        if (result.IsSuccess) return TypedResults.Stream(result.Value, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);
        
        var problemDetailsInfo = ProblemDetailsMap.Find(failureStatusCode);
        var problemDetails = new ProblemDetails
        {
            Status = failureStatusCode,
            Title = problemDetailsInfo.Title,
            Type = problemDetailsInfo.Type,
            Detail = result.Error
        };
        
        return TypedResults.Problem(problemDetails);
    }
    
    /// <summary>
    /// Returns a <see cref="FileStreamHttpResult"/> based of a Stream in case of success result. Returns <see cref="ProblemHttpResult"/> in case of failure. You can override the error status code.
    /// </summary>
    public static async Task<Results<FileStreamHttpResult, ProblemHttpResult>> ToFileStreamHttpResult<T>(this Task<Result<T>> result, string? contentType = null,
        string? fileDownloadName = null, DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null,
        bool enableRangeProcessing = false, int failureStatusCode = 400) where T : Stream
    {
        return (await result).ToFileStreamHttpResult(contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing, failureStatusCode);
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace CSharpFunctionalExtensions.HttpResults.ResultExtensions;

public static partial class ResultExtensions
{
    public static Microsoft.AspNetCore.Http.IResult ToFileHttpResult(this Result<byte[]> result, string? contentType = null,
        string? fileDownloadName = null, DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null,
        bool enableRangeProcessing = false, int failureStatusCode = 400)
    {
        if (result.IsSuccess) return TypedResults.File(result.Value, contentType, fileDownloadName, enableRangeProcessing, lastModified, entityTag);
        
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
    
    public static async Task<Microsoft.AspNetCore.Http.IResult> ToFileHttpResult(this Task<Result<byte[]>> result, string? contentType = null,
        string? fileDownloadName = null, DateTimeOffset? lastModified = null,
        EntityTagHeaderValue? entityTag = null, bool enableRangeProcessing = false, int failureStatusCode = 400)
    {
        return (await result).ToFileHttpResult(contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing, failureStatusCode);
    }
}
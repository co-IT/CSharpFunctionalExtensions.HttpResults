using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.ResultExtensions;

/// <summary>
/// Extension methods for <see cref="Result{T}"/>
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Discards the value of <see cref="Result{T}"/> and Returns a <see cref="NoContent"/> in case of success result. Returns <see cref="ProblemHttpResult"/> in case of failure. You can override the error status code.
    /// </summary>
    public static Results<NoContent, ProblemHttpResult> ToNoContentHttpResult<T>(this Result<T> result, int failureStatusCode = 400)
    {
        if (result.IsSuccess) return TypedResults.NoContent();
        
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
    /// Discards the value of <see cref="Result{T}"/> and Returns a <see cref="NoContent"/> in case of success result. Returns <see cref="ProblemHttpResult"/> in case of failure. You can override the error status code.
    /// </summary>
    public static async Task<Results<NoContent, ProblemHttpResult>> ToNoContentHttpResult<T>(this Task<Result<T>> result, int failureStatusCode = 400)
    {
        return (await result).ToNoContentHttpResult(failureStatusCode);
    }
}
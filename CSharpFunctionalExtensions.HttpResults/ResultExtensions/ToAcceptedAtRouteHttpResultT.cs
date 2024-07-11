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
    /// Returns a <see cref="AcceptedAtRoute{TValue}"/> with Accepted status code in case of success result. Returns <see cref="ProblemHttpResult"/> in case of failure. You can provide route info to create a location HTTP-Header. You can override the error status code.
    /// </summary>
    public static Results<AcceptedAtRoute<T>, ProblemHttpResult> ToAcceptedAtRouteHttpResult<T>(this Result<T> result, string? routeName = null, Func<T, object>? routeValues = null, int failureStatusCode = 400)
    {
        if (result.IsSuccess) return TypedResults.AcceptedAtRoute(result.Value, routeName, routeValues?.Invoke(result.Value));
        
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
    /// Returns a <see cref="AcceptedAtRoute{TValue}"/> with Accepted status code in case of success result. Returns <see cref="ProblemHttpResult"/> in case of failure. You can provide route info to create a location HTTP-Header. You can override the error status code.
    /// </summary>
    public static async Task<Results<AcceptedAtRoute<T>, ProblemHttpResult>> ToAcceptedAtRouteHttpResult<T>(this Task<Result<T>> result, string? routeName = null, Func<T, object>? routeValues = null, int failureStatusCode = 400)
    {
        return (await result).ToAcceptedAtRouteHttpResult(routeName, routeValues, failureStatusCode);
    }
}
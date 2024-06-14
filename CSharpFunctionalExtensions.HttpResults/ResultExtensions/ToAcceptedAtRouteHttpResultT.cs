using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.ResultExtensions;

public static partial class ResultExtensions
{
    public static Microsoft.AspNetCore.Http.IResult ToAcceptedAtRouteHttpResult<T>(this Result<T> result, string? routeName = null, object? routeValues = null, int failureStatusCode = 400)
    {
        if (result.IsSuccess) return TypedResults.AcceptedAtRoute(result.Value, routeName, routeValues);
        
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
    
    public static async Task<Microsoft.AspNetCore.Http.IResult> ToAcceptedAtHttpResult<T>(this Task<Result<T>> result, string? routeName = null, object? routeValues = null, int failureStatusCode = 400)
    {
        return (await result).ToAcceptedAtRouteHttpResult(routeName, routeValues, failureStatusCode);
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.ResultExtensions;

public static partial class ResultExtensions
{
    public static Microsoft.AspNetCore.Http.IResult ToCreatedHttpResult<T>(this Result<T> result, Uri? uri = null, int failureStatusCode = 400)
    {
        if (result.IsSuccess) return TypedResults.Created(uri, result.Value);
        
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
    
    public static async Task<Microsoft.AspNetCore.Http.IResult> ToCreatedHttpResult<T>(this Task<Result<T>> result, Uri? uri = null, int failureStatusCode = 400)
    {
        return (await result).ToCreatedHttpResult(uri, failureStatusCode);
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.ResultExtensions;

public static partial class ResultExtensions
{
    public static Microsoft.AspNetCore.Http.IResult ToNoContentHttpResult<T>(this Result<T> result, int successStatusCode = 204, int failureStatusCode = 400)
    {
        if (result.IsSuccess) return TypedResults.StatusCode(successStatusCode);
        
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
    
    public static async Task<Microsoft.AspNetCore.Http.IResult> ToNoContentHttpResult<T>(this Task<Result<T>> result, int successStatusCode = 200, int failureStatusCode = 400)
    {
        return (await result).ToHttpResult(successStatusCode, failureStatusCode);
    }
}
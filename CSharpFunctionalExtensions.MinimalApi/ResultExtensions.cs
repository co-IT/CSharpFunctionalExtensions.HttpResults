using Microsoft.AspNetCore.Http;

namespace CSharpFunctionalExtensions.MinimalApi;

public static class ResultExtensions
{
    public static Microsoft.AspNetCore.Http.IResult ToHttpResult<T>(this Result<T, int> result)
    {
        return TypedResults.Ok(result.Value);
    }
}
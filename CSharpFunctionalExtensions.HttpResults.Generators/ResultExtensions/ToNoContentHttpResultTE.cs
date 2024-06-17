namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToNoContentHttpResultTE: IGenerateMethods
{
    public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 /// <summary>
                 /// Discards the value of <see cref="Result{T}"/> and Returns a <see cref="StatusCodeHttpResult"/> in case of success result. Returns custom mapping in case of failure. You can override the success status code.
                 /// </summary>
                 public static {{httpResultType}} ToNoContentHttpResult<T>(this Result<T,{{resultErrorType}}> result, int successStatusCode = 204)
                 {
                     if (result.IsSuccess) return TypedResults.StatusCode(successStatusCode);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 /// <summary>
                 /// Discards the value of <see cref="Result{T}"/> and Returns a <see cref="StatusCodeHttpResult"/> in case of success result. Returns custom mapping in case of failure. You can override the success status code.
                 /// </summary>
                 public static async Task<{{httpResultType}}> ToNoContentHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, int successStatusCode = 200)
                 {
                     return (await result).ToHttpResult(successStatusCode);
                 }
                 """;
    }
}
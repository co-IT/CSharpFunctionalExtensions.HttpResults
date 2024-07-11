using CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

namespace CSharpFunctionalExtensions.HttpResults.Generators.UnitResultExtensions;

internal class ToHttpResultE: IGenerateMethods
{
    public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 /// <summary>
                 /// Returns a <see cref="StatusCodeHttpResult"/> in case of success result. Returns custom mapping in case of failure. You can override the success status code.
                 /// </summary>
                 public static Results<StatusCodeHttpResult, {{httpResultType}}> ToHttpResult(this UnitResult<{{resultErrorType}}> result, int successStatusCode = 204)
                 {
                     if (result.IsSuccess) return TypedResults.StatusCode(successStatusCode);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 /// <summary>
                 /// Returns a <see cref="StatusCodeHttpResult"/> in case of success result. Returns custom mapping in case of failure. You can override the success status code.
                 /// </summary>
                 public static async Task<Results<StatusCodeHttpResult, {{httpResultType}}>> ToHttpResult(this Task<UnitResult<{{resultErrorType}}>> result, int successStatusCode = 204)
                 {
                     return (await result).ToHttpResult(successStatusCode);
                 }
                 """;
    }
}
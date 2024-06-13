namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

public partial class ResultExtensionsGenerator
{
    public static string ToHttpResultTE(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 public static {{httpResultType}} ToHttpResult<T>(this Result<T,{{resultErrorType}}> result, int successStatusCode = 200)
                 {
                     if (result.IsSuccess) return TypedResults.Json(result.Value, statusCode: successStatusCode);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 public static async Task<{{httpResultType}}> ToHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, int successStatusCode = 200)
                 {
                     return (await result).ToHttpResult(successStatusCode);
                 }
                 """;
    }
}
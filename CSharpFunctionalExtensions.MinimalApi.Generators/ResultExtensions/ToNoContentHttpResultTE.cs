namespace CSharpFunctionalExtensions.MinimalApi.Generators.ResultExtensions;

public partial class ResultExtensionsGenerator
{
    public static string ToNoContentHttpResultTE(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                    public static {{httpResultType}} ToNoContentHttpResult<T>(this Result<T,{{resultErrorType}}> result, int successStatusCode = 204, int failureStatusCode = 400)
                 {
                     if (result.IsSuccess) return TypedResults.StatusCode(successStatusCode);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 public static async Task<{{httpResultType}}> ToNoContentHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, int successStatusCode = 200, int failureStatusCode = 400)
                 {
                     return (await result).ToHttpResult(successStatusCode, failureStatusCode);
                 }
                 """;
    }
}
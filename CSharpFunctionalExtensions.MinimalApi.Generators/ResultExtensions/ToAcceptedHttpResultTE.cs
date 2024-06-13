namespace CSharpFunctionalExtensions.MinimalApi.Generators.ResultExtensions;

public partial class ResultExtensionsGenerator
{
    public static string ToAcceptedHttpResultTE(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 public static {{httpResultType}} ToAcceptedHttpResult<T>(this Result<T,{{resultErrorType}}> result, Uri uri, int failureStatusCode = 400)
                 {
                     if (result.IsSuccess) return TypedResults.Accepted(uri, result.Value);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 public static async Task<{{httpResultType}}> ToAcceptedHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, Uri uri, int failureStatusCode = 400)
                 {
                     return (await result).ToAcceptedHttpResult(uri, failureStatusCode);
                 }
                 """;
    }
}
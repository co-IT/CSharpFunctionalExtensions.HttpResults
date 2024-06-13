namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

public partial class ResultExtensionsGenerator
{
    public static string ToCreatedHttpResultTE(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 public static {{httpResultType}} ToCreatedHttpResult<T>(this Result<T,{{resultErrorType}}> result, Uri? uri = null)
                 {
                     if (result.IsSuccess) return TypedResults.Created(uri, result.Value);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 public static async Task<{{httpResultType}}> ToCreatedHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, Uri? uri = null)
                 {
                     return (await result).ToCreatedHttpResult(uri);
                 }
                 """;
    }
}
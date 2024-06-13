namespace CSharpFunctionalExtensions.HttpResults.Generators.UnitResultExtensions;

public class UnitResultExtensionsGenerator
{
    public static string ToHttpResultE(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 public static {{httpResultType}} ToHttpResult(this UnitResult<{{resultErrorType}}> result, int successStatusCode = 204)
                 {
                     if (result.IsSuccess) return TypedResults.StatusCode(successStatusCode);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 public static async Task<{{httpResultType}}> ToHttpResult(this Task<UnitResult<{{resultErrorType}}>> result, int successStatusCode = 204)
                 {
                     return (await result).ToHttpResult(successStatusCode);
                 }
                 """;
    }
}
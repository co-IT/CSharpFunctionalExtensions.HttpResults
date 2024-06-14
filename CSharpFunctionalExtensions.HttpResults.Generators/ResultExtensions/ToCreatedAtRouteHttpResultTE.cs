namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

public partial class ResultExtensionsGenerator
{
    public static string ToCreatedAtRouteHttpResultTE(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 public static {{httpResultType}} ToCreatedAtRouteHttpResult<T>(this Result<T,{{resultErrorType}}> result, string? routeName = null, object? routeValues = null)
                 {
                     if (result.IsSuccess) return TypedResults.CreatedAtRoute(result.Value, routeName, routeValues);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 public static async Task<{{httpResultType}}> ToCreatedAtHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, string? routeName = null, object? routeValues = null)
                 {
                     return (await result).ToCreatedAtRouteHttpResult(routeName, routeValues);
                 }
                 """;
    }
}
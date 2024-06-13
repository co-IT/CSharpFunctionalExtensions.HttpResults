namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

public partial class ResultExtensionsGenerator
{
    public static string ToAcceptedAtRouteHttpResultTE(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 public static {{httpResultType}} ToAcceptedAtRouteHttpResult<T>(this Result<T,{{resultErrorType}}> result, string? routeName = null, object? routeValues = null)
                 {
                     if (result.IsSuccess) return TypedResults.AcceptedAtRoute(result.Value, routeName, routeValues);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 public static async Task<{{httpResultType}}> ToAcceptedAtHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, string? routeName = null, object? routeValues = null)
                 {
                     return (await result).ToAcceptedAtRouteHttpResult(routeName, routeValues);
                 }
                 """;
    }
}
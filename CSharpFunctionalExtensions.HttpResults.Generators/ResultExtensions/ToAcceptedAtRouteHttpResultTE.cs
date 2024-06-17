namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToAcceptedAtRouteHttpResultTE: IGenerateMethods
{
    public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 /// <summary>
                 /// Returns a <see cref="AcceptedAtRoute{TValue}"/> with Accepted status code in case of success result. Returns custom mapping in case of failure. You can provide route info to create a location HTTP-Header.
                 /// </summary>
                 public static {{httpResultType}} ToAcceptedAtRouteHttpResult<T>(this Result<T,{{resultErrorType}}> result, string? routeName = null, object? routeValues = null)
                 {
                     if (result.IsSuccess) return TypedResults.AcceptedAtRoute(result.Value, routeName, routeValues);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 /// <summary>
                 /// Returns a <see cref="AcceptedAtRoute{TValue}"/> with Accepted status code in case of success result. Returns custom mapping in case of failure. You can provide route info to create a location HTTP-Header.
                 /// </summary>
                 public static async Task<{{httpResultType}}> ToAcceptedAtHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, string? routeName = null, object? routeValues = null)
                 {
                     return (await result).ToAcceptedAtRouteHttpResult(routeName, routeValues);
                 }
                 """;
    }
}
namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToCreatedAtRouteHttpResultTE: IGenerateMethods
{
    public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 /// <summary>
                 /// Returns a <see cref="CreatedAtRoute{TValue}"/> with Created status code in case of success result. Returns custom mapping in case of failure. You can provide route info to create a location HTTP-Header.
                 /// </summary>
                 public static Results<CreatedAtRoute<T>, {{httpResultType}}> ToCreatedAtRouteHttpResult<T>(this Result<T,{{resultErrorType}}> result, string? routeName = null, Func<T, object>? routeValues = null)
                 {
                     if (result.IsSuccess) return TypedResults.CreatedAtRoute(result.Value, routeName, routeValues?.Invoke(result.Value));
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 /// <summary>
                 /// Returns a <see cref="CreatedAtRoute{TValue}"/> with Created status code in case of success result. Returns custom mapping in case of failure. You can provide route info to create a location HTTP-Header.
                 /// </summary>
                 public static async Task<Results<CreatedAtRoute<T>, {{httpResultType}}>> ToCreatedAtRouteHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, string? routeName = null, Func<T, object>? routeValues = null)
                 {
                     return (await result).ToCreatedAtRouteHttpResult(routeName, routeValues);
                 }
                 """;
    }
}
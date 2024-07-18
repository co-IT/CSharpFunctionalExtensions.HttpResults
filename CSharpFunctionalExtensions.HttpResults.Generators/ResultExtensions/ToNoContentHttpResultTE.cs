namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToNoContentHttpResultTE: IGenerateMethods
{
    public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 /// <summary>
                 /// Discards the value of <see cref="Result{T,E}"/> and Returns a <see cref="NoContent"/> in case of success result. Returns custom mapping in case of failure.
                 /// </summary>
                 public static Results<NoContent, {{httpResultType}}> ToNoContentHttpResult<T>(this Result<T,{{resultErrorType}}> result)
                 {
                     if (result.IsSuccess) return TypedResults.NoContent();
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 /// <summary>
                 /// Discards the value of <see cref="Result{T,E}"/> and Returns a <see cref="NoContent"/> in case of success result. Returns custom mapping in case of failure.
                 /// </summary>
                 public static async Task<Results<NoContent, {{httpResultType}}>> ToNoContentHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result)
                 {
                     return (await result).ToNoContentHttpResult();
                 }
                 """;
    }
}